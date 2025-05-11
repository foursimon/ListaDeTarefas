using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backend.Infraestrutura
{
	public static class AuthorizationConfig
	{
		public static void AddJwtAuthentication(this WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = builder.Configuration["JWT:Issuer"],
						ValidateAudience = true,
						ValidAudience = builder.Configuration["JWT:Audience"],
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(builder.Configuration["JWT:Token"]!)),
						ValidateIssuerSigningKey = true
					};
					//Aplicando configurações de Cookies para acessar o token de acesso.
					opt.Events = new JwtBearerEvents
					{
						OnMessageReceived = contexto =>
						{
							contexto.Token = contexto.Request.Cookies["TOKEN_ACESSO"];
							return Task.CompletedTask;
						}
					};
				});
		}
	}
}
