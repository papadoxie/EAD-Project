using PUCCI.Models.Audit;
using PUCCI.Models.Interfaces;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using System.CodeDom;

namespace PUCCI.Models
{
    public class Container : AuditModel, IContainer
    {
        public async void Create(string imageID)
        {
            Image = imageID;

            var client = new DockerClientConfiguration()
                .CreateClient();

            var image = await GetImageFromID(client);

            var container = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = image.RepoTags.First(),
                HostConfig= new HostConfig()
                {
                    DNS = new[] {"8.8.8.8", "8.8.4.4"}
                }
            });

            var id = container.ID;
            ContainerID = id;
        }

        public async void Run()
        {
            var client = new DockerClientConfiguration()
                .CreateClient();

            await client.Containers.StartContainerAsync(
                ContainerID,
                new ContainerStartParameters()
                );
            SetContainerDetails(client);
        }

        private async void SetContainerDetails(DockerClient client)
        {
            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { });

            var container = from ctr in containers
                        where ctr.ID == ContainerID
                        select (ctr.Names.FirstOrDefault("unknown"), ctr.Status);

            Name = container.First().Item1;
            Status = container.First().Item2; 
        }

        private async Task<ImagesListResponse> GetImageFromID(DockerClient client)
        {
            var images = await client.Images.ListImagesAsync(new ImagesListParameters() { });

            var id = Image;
            var image = from img in images
                        where img.ID == Image
                        select img;
            return image.First();
        }

        public int? ID { get; set; }
        public string? ContainerID { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
    }
}
