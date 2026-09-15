using FluentValidation;
using GramaMaster.Application.DTOs.Authentication;
using GramaMaster.Application.DTOs.Common;
using GramaMaster.Application.Interfaces.Persistence;
using GramaMaster.Application.Interfaces.Security;
using GramaMaster.Application.Interfaces.Services;
using GramaMaster.Application.Validators.Auth;
using GramaMaster.Domain.Entities;
using GramaMaster.Domain.Enums;
using System.Security.Cryptography;

namespace GramaMaster.Application.Services
{
    /// <summary>
    /// Authentication service managing student & teacher registration, authentication, 
    /// password management, refresh token rotation, and email verification workflows.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        private readonly IValidator<RegisterStudentDto> _registerStudentValidator;
        private readonly IValidator<RegisterTeacherDto> _registerTeacherValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
        private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
        private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IEmailService emailService,
            IValidator<RegisterStudentDto>? registerStudentValidator = null,
            IValidator<RegisterTeacherDto>? registerTeacherValidator = null,
            IValidator<LoginDto>? loginValidator = null,
            IValidator<ChangePasswordDto>? changePasswordValidator = null,
            IValidator<ForgotPasswordDto>? forgotPasswordValidator = null,
            IValidator<ResetPasswordDto>? resetPasswordValidator = null)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));

            _registerStudentValidator = registerStudentValidator ?? new RegisterStudentValidator();
            _registerTeacherValidator = registerTeacherValidator ?? new RegisterTeacherValidator();
            _loginValidator = loginValidator ?? new LoginValidator();
            _changePasswordValidator = changePasswordValidator ?? new ChangePasswordValidator();
            _forgotPasswordValidator = forgotPasswordValidator ?? new ForgotPasswordValidator();
            _resetPasswordValidator = resetPasswordValidator ?? new ResetPasswordValidator();
        }

        public async Task<ApiResponse<LoginResponseDto>> RegisterStudentAsync(RegisterStudentDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Request payload cannot be empty" },
                    "Invalid request");
            }

            // 1. Validate DTO via FluentValidation
            var validationResult = await _registerStudentValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    validationResult.Errors.Select(e => e.ErrorMessage),
                    "Validation failed");
            }

            // 2. Check Password Confirmation
            if (dto.Password != dto.ConfirmPassword)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Password and confirm password do not match" },
                    "Password mismatch");
            }

            // 3. Email Normalization & Uniqueness Check
            var email = dto.Email.Trim().ToLowerInvariant();
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
            if (existingUser != null)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { $"User with email '{dto.Email}' already exists" },
                    "Email already registered");
            }

            // 4. Curriculum Lookup & Auto-Seed Fallback
            var curriculumName = dto.CurriculumType.ToString();
            var curriculums = await _unitOfWork.Curriculums.FindAsync(c =>
                c.Name.ToLower().Contains(curriculumName.ToLower()));

            var curriculum = curriculums.FirstOrDefault();
            if (curriculum == null)
            {
                curriculum = new Curriculum
                {
                    Id = Guid.NewGuid(),
                    Name = curriculumName,
                    Description = $"Default Curriculum for {curriculumName}",
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Curriculums.AddAsync(curriculum);
            }

            // 5. Build User Entity
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName.Trim(),
                Email = email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = UserRole.Student,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // 6. Build Student Entity
            var student = new Student
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                User = user,
                ProfileImageUrl = dto.ProfileImageUrl?.Trim(),
                InstitutionName = dto.InstitutionName.Trim(),
                CurriculumType = dto.CurriculumType,
                CurriculumId = curriculum.Id,
                Curriculum = curriculum,
                Group = dto.Group,
                ExamBatchYear = dto.ExamBatchYear,
                Board = dto.Board,
                CreatedAt = DateTime.UtcNow
            };

            user.Student = student;

            // 7. Token Generation & Persistence
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiry();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            // 8. Commit Transaction
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // 9. Build Response
            var response = new LoginResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Student registered successfully");
        }

        public async Task<ApiResponse<LoginResponseDto>> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Request payload cannot be empty" },
                    "Invalid request");
            }

            // 1. Validate DTO via FluentValidation
            var validationResult = await _registerTeacherValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    validationResult.Errors.Select(e => e.ErrorMessage),
                    "Validation failed");
            }

            // 2. Check Password Confirmation
            if (dto.Password != dto.ConfirmPassword)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Password and confirm password do not match" },
                    "Password mismatch");
            }

            // 3. Email Normalization & Uniqueness Check
            var email = dto.Email.Trim().ToLowerInvariant();
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
            if (existingUser != null)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { $"User with email '{dto.Email}' already exists" },
                    "Email already registered");
            }

            // 4. Build User Entity
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName.Trim(),
                Email = email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = UserRole.Teacher,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // 5. Build Teacher Entity
            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                User = user,
                ProfileImageUrl = dto.ProfileImageUrl?.Trim(),
                SscResult = dto.SscResult,
                SscGroup = dto.SscGroup,
                SscBoard = dto.SscBoard,
                HscResult = dto.HscResult,
                HscGroup = dto.HscGroup,
                HscBoard = dto.HscBoard,
                Department = dto.Department.Trim(),
                University = dto.University.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            user.Teacher = teacher;

            // 6. Token Generation & Persistence
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiry();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;

            // 7. Commit Transaction
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // 8. Build Response
            var response = new LoginResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Teacher registered successfully");
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Request payload cannot be empty" },
                    "Invalid request");
            }

            // 1. Validate DTO
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    validationResult.Errors.Select(e => e.ErrorMessage),
                    "Validation failed");
            }

            // 2. Fetch User by Normalized Email
            var email = dto.Email.Trim().ToLowerInvariant();
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Invalid email or password" },
                    "Invalid credentials");
            }

            // 3. Account Status Verification
            if (!user.IsActive)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Your account has been disabled. Please contact support." },
                    "Account disabled");
            }

            // 4. Token Generation & Refresh Token Update
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiry();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // 5. Build Response
            var response = new LoginResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful");
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
        {
            if (dto == null || userId == Guid.Empty)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "Invalid request data" },
                    "Invalid input");
            }

            // 1. Validate DTO
            var validationResult = await _changePasswordValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<bool>.ErrorResponse(
                    validationResult.Errors.Select(e => e.ErrorMessage),
                    "Validation failed");
            }

            // 2. Password Confirmation Match
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "New password and confirm password do not match" },
                    "Password mismatch");
            }

            // 3. Find User
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "User does not exist" },
                    "User not found");
            }

            // 4. Verify Current Password
            if (!_passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "Current password is incorrect" },
                    "Invalid password");
            }

            // 5. Update Password
            user.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Password changed successfully");
        }

        public async Task<ApiResponse<bool>> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "Email is required" },
                    "Invalid input");
            }

            // 1. Validate DTO
            var validationResult = await _forgotPasswordValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<bool>.ErrorResponse(
                    validationResult.Errors.Select(e => e.ErrorMessage),
                    "Validation failed");
            }

            const string genericSuccessMessage = "If an account with this email exists, a password reset link has been sent.";

            // 2. Fetch User by Normalized Email
            var email = dto.Email.Trim().ToLowerInvariant();
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                // Prevent user enumeration attacks by returning generic success
                return ApiResponse<bool>.SuccessResponse(true, genericSuccessMessage);
            }

            // 3. Generate Cryptographically Secure Reset Token
            var resetTokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(resetTokenBytes);
            }
            var resetToken = Convert.ToHexString(resetTokenBytes);

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(1);
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // 4. Dispatch Email
            try
            {
                await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);
            }
            catch
            {
                // Log error if logger present; return generic success to caller
                return ApiResponse<bool>.SuccessResponse(true, genericSuccessMessage);
            }

            return ApiResponse<bool>.SuccessResponse(true, genericSuccessMessage);
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto == null)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "Reset data is required" },
                    "Invalid input");
            }

            // 1. Validate DTO
            var validationResult = await _resetPasswordValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return ApiResponse<bool>.ErrorResponse(
                    validationResult.Errors.Select(e => e.ErrorMessage),
                    "Validation failed");
            }

            // 2. Verify Password Confirmation
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "New password and confirm password do not match" },
                    "Password mismatch");
            }

            // 3. Fetch User by Email
            var email = dto.Email.Trim().ToLowerInvariant();
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "Invalid reset request" },
                    "Invalid request");
            }

            // 4. Validate Stored Reset Token & Expiration
            if (string.IsNullOrWhiteSpace(user.PasswordResetToken) ||
                user.PasswordResetToken != dto.Token ||
                !user.PasswordResetTokenExpiryTime.HasValue ||
                user.PasswordResetTokenExpiryTime.Value < DateTime.UtcNow)
            {
                return ApiResponse<bool>.ErrorResponse(
                    new[] { "Reset token is invalid or has expired" },
                    "Invalid token");
            }

            // 5. Update Password & Invalidate Reset Token
            user.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiryTime = null;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Password reset successfully");
        }

        public async Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Refresh token is required" },
                    "Invalid token");
            }

            // 1. Look up User by Active Refresh Token
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);

            if (user == null ||
                !user.IsActive ||
                !user.RefreshTokenExpiryTime.HasValue ||
                user.RefreshTokenExpiryTime.Value < DateTime.UtcNow)
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    new[] { "Refresh token is invalid or has expired" },
                    "Invalid token");
            }

            // 2. Perform Refresh Token Rotation
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenService.GetRefreshTokenExpiry();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // 3. Build Response
            var response = new LoginResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Token refreshed successfully");
        }
    }
}
