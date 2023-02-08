namespace PUCCI.Models.Audit
{
	public class CurrentUserService : ICurrentUserService
	{

		public CurrentUserService(IHttpContextAccessor contextAccessor) 
		{
			_httpContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof (ArgumentNullException));
		}

		public string GetCurrentUsername()
		{
			return _httpContextAccessor.HttpContext!.User.Identity!.Name!;
		}

		private readonly IHttpContextAccessor _httpContextAccessor;
	}
}
