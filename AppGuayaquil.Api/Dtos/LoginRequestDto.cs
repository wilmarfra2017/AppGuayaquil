﻿namespace AppGuayaquil.Api.Dtos;

public class LoginRequestDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}