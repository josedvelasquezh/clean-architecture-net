using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{
    public class GetVideosListQuery : IRequest<List<VideosVm>>
    {
        public string _username { get; set; } = string.Empty;

        public GetVideosListQuery(string username)
        {
            _username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
