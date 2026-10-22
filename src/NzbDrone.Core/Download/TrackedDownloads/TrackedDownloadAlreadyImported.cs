using System.Collections.Generic;
using System.Linq;
using NLog;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.History;
using NzbDrone.Core.Music;

namespace NzbDrone.Core.Download.TrackedDownloads
{
    public interface ITrackedDownloadAlreadyImported
    {
        bool IsImported(TrackedDownload trackedDownload, List<EntityHistory> historyItems);
    }

    public class TrackedDownloadAlreadyImported : ITrackedDownloadAlreadyImported
    {
        private readonly Logger _logger;
        private readonly ITrackService _trackService;

        public TrackedDownloadAlreadyImported(ITrackService trackService, Logger logger)
        {
            _trackService = trackService;
            _logger = logger;
        }

        public bool IsImported(TrackedDownload trackedDownload, List<EntityHistory> historyItems)
        {
            _logger.Trace("Checking if all items for '{0}' have been imported", trackedDownload.DownloadItem.Title);

            if (historyItems.Empty())
            {
                _logger.Trace("No history for {0}", trackedDownload.DownloadItem.Title);
                return false;
            }

            if (trackedDownload.RemoteAlbum == null)
            {
                return true;
            }

            if (trackedDownload.RemoteAlbum.TargetRecordingIds?.Any() == true)
            {
                var importedHistory = historyItems.Where(item => item.EventType == EntityHistoryEventType.TrackFileImported).ToList();
                var importedRecordingIds = importedHistory.Select(item => item.Data.GetValueOrDefault("RecordingId"))
                    .Where(id => id.IsNotNullOrWhiteSpace())
                    .ToHashSet();
                var legacyTrackIds = importedHistory.Where(item => item.Data.GetValueOrDefault("RecordingId").IsNullOrWhiteSpace())
                    .Select(item => item.TrackId).Distinct().ToList();

                if (legacyTrackIds.Any())
                {
                    importedRecordingIds.UnionWith(_trackService.GetTracks(legacyTrackIds).Select(track => track.ForeignRecordingId));
                }

                return trackedDownload.RemoteAlbum.TargetRecordingIds.All(importedRecordingIds.Contains);
            }

            if (trackedDownload.RemoteAlbum.Albums == null)
            {
                return true;
            }

            var allAlbumsImportedInHistory = trackedDownload.RemoteAlbum.Albums.All(album =>
            {
                var lastHistoryItem = historyItems.FirstOrDefault(h => h.AlbumId == album.Id);

                if (lastHistoryItem == null)
                {
                    _logger.Trace($"No history for album: {album}");
                    return false;
                }

                _logger.Trace($"Last event for album: {album} is: {lastHistoryItem.EventType}");

                return new[] { EntityHistoryEventType.DownloadImported, EntityHistoryEventType.TrackFileImported }.Contains(lastHistoryItem.EventType);
            });

            _logger.Trace("All albums for '{0}' have been imported: {1}", trackedDownload.DownloadItem.Title, allAlbumsImportedInHistory);

            return allAlbumsImportedInHistory;
        }
    }
}
