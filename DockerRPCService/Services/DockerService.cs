using DockerRPCService;
using Grpc.Core;

namespace DockerRPCService.Services
{
	public class DockerImageService : DockerImage.DockerImageBase
	{
		private readonly ILogger<DockerImageService> _logger;
		public DockerImageService(ILogger<DockerImageService> logger)
		{
			_logger = logger;
		}

		public override Task<Image> CreateImage(Dockerfile request, ServerCallContext context)
		{
			return Task.FromResult(new Image
			{
			});
		}
	}
}