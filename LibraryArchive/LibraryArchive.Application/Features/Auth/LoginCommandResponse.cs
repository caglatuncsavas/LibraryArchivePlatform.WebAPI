﻿namespace LibraryArchive.Application.Features.Auth;
public sealed record LoginCommandResponse(
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpires);