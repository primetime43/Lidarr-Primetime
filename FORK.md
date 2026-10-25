# Song Mode and fork maintenance

Song Mode lets you choose any song or several songs from an album. A song does not need to have been released as a single. The requested MusicBrainz recording IDs define what is kept, including when a recording appears on multiple albums.

## Choose songs

Open an artist and click the visible **Songs** tab or toolbar shortcut. **Albums** remains available alongside it. Songs also has a direct link at `/artist/{foreignArtistId}?view=songs`, and browser Back/Forward follows view changes. Switching views preserves the current song draft; selecting another artist starts that artist's own checklist.

Check the songs you want and click **Download selected**. This starts a one-time song search without changing album monitoring, artist monitoring, or the artist's monitoring policy. For artists using normal album monitoring, the song checklist starts empty rather than inheriting album defaults. **Clear selection** starts a fresh checklist. Checked songs hidden by the filter remain selected. A status message links to **Activity → Queue** after the search starts.

Artists already configured for ongoing Song Mode monitoring retain their saved song selections and an additional **Save monitored songs** action. Saving updates only the checked songs' monitoring without starting a search. Downloading is separate from saving. Enabling that monitoring policy clears inherited defaults when every track is monitored and preserves existing partial selections. Existing music and in-flight downloads are preserved.

The Albums view keeps its normal album controls, including monitored album searches, even when the artist uses song monitoring. Whole-album grabs and targeted song grabs keep distinct import intent, so requesting an album imports the album while requesting a few songs imports only those recordings. Merely opening either view changes no settings.

Click a song row or its checkbox to select it; Shift-click selects a range in the displayed order. Focus a row and press Space or Enter to toggle it with the keyboard. Buttons and links keep their normal actions.

The **Library status** column marks songs as **Downloaded** or **Missing**. Downloaded rows have a subtle green tint and remain selectable for upgrade searches. The header shows downloaded and missing counts, and the **All songs / Missing / Downloaded** filter narrows the list without discarding hidden selections. Availability follows the recording across its album appearances; a different live recording or remix is tracked separately.

The **Popularity** column shows each matched song's position in the artist's Deezer top tracks, with a link to the source. **Most popular** sorts those songs first; title and album sorting are also available. These are provider rankings, not community ratings or a measure of song quality. A dash means no confident match, not a bad song. Catalog matching checks title and duration, preserves live/remix versions, and requires multiple recordings to identify same-named artists unless metadata already supplies a Deezer artist link. Public catalog lookups need no API key and are cached for 12 hours; provider failures never block song selection or downloading.

Selective downloads show **Wanted songs** below the album title in **Activity → Queue**. These titles come from the recordings saved with the download, so later checklist changes do not mislabel an in-flight download. Normal whole-album downloads keep their usual display.

Searches query the artist and song title first. Newznab-compatible indexers use their track parameter when supported, or a text query otherwise. Results must match the requested artist and song title; import identification then checks recording IDs. Availability depends on what the configured indexers publish and how their releases are named.

If a song download is unavailable or rejected, the search can use a release containing the recording: single releases, EPs, ordinary albums, compilations, other release types, and finally discographies. These are download sources for the selected songs. Each candidate must pass the configured quality and release requirements. Among accepted candidates, song downloads take precedence over larger releases; quality preferences apply within each source tier.

Only the requested recordings are imported. Completion requires every requested recording, so duplicate copies of one song cannot stand in for a second song. A track file containing both wanted and unwanted recordings may remain intact; this fork does not split audio files.

After all requested songs in a movable download are imported, files positively identified as untargeted are discarded from that download. Unidentified files and incomplete imports remain available for inspection. Seeding torrents retain their source files until the download client permits removal. Enable **Remove Completed** for the download client to remove completed downloads and their remaining source data under Lidarr's usual rules. Unwanted songs are not added to the music library while waiting for seeding to finish.

## Aurral integration

Existing Lidarr v1 artist, album, command, and queue endpoints remain available. Point Aurral's Lidarr URL and API key at this fork as usual, with consistent library and download mounts.

[Aurral currently downloads individual tracks itself](https://github.com/lklynet/aurral/blob/main/docs/src/content/docs/integrations/lidarr.mdx), including playlist and flow tracks. Its [Lidarr client](https://github.com/lklynet/aurral/blob/main/backend/services/lidarrClient.js) sends `AlbumSearch` and `ArtistSearch`; those actions continue to have their existing meanings. Enabling Song Mode in this fork does not turn an Aurral album request into a song request.

To route Aurral song requests through this fork, a future Aurral change can use the following API contract. All requests use the normal `X-Api-Key` header and `/api/v1` prefix.

1. Probe `GET /track/capabilities`. This fork returns `{"trackSearch":true,"selectiveImport":true}`. A stock Lidarr instance can return 404; Aurral should retain its existing provider flow in that case.
2. Ensure the artist and album metadata exist using the normal Lidarr APIs. Leave an album unmonitored when only selected songs are wanted. For ongoing per-song monitoring, set `songMode: true` on the artist through the existing artist update API.
3. Fetch `GET /track?albumId=123` or `GET /track?artistId=456`. Match the requested MusicBrainz recording ID to `foreignRecordingId`, then use the returned numeric `id`. Track IDs and recording IDs are different identifiers.
4. For ongoing monitoring, send `PUT /track/monitor` with `{"trackIds":[101,102],"monitored":true}`. Newly monitored tracks trigger a search. Monitor only the wanted songs across the artist's catalog; enabling Song Mode preserves existing monitoring selections.
5. For an explicit one-time search, send `POST /command` with `{"name":"TrackSearch","trackIds":[101,102]}`. This works independently of whether those tracks are monitored. Avoid sending this immediately after a monitor update that already triggered a search.
6. Follow the returned command ID and the existing queue/history APIs for progress.

Aurral itself has not been modified in this repository. Its song-request integration and an end-to-end test with a running Aurral instance are separate work.

External transfer tools should extract archives in a per-download staging directory, then send `DownloadedAlbumsScan` with the extracted directory's container-visible `path`, the original `downloadClientId`, and `importMode: "Copy"`. The download ID preserves the wanted recording list through import and completion checks. Keep staging data until history confirms `downloadImported`; a completed transfer or disappeared queue row alone does not confirm a library import. Packed archive files are reported with extraction instructions.

## Keep the fork current

The remotes for this checkout are `origin` (`primetime43/Lidarr-Primetime`) and `upstream` (`Lidarr/Lidarr`). Keep custom features on the fork's `develop` branch and merge upstream `develop` into it; preserve the shared history for future merges.

The **Sync upstream Lidarr** GitHub Actions workflow runs daily and supports manual runs. It merges upstream into the reserved `automation/sync-upstream` branch, checks for duplicate migration versions, runs the Song Mode regression tests, and opens or refreshes a pull request into `develop`. Review and merge that pull request to update the fork. Conflicts or failed tests stop the workflow and leave `develop` unchanged. The automation branch is regenerated, so do not put manual feature work on it.

After pushing these workflow files, enable Actions for the fork and allow GitHub Actions to create pull requests in **Settings > Actions > General > Workflow permissions**. Scheduled workflows run from the repository's default branch; the workflow must be present there. Repository or organization policies can restrict these settings. [Pull-request workflows created by `GITHUB_TOKEN` can require approval](https://docs.github.com/en/actions/concepts/security/github_token), so the sync workflow runs its checks before opening the pull request.

For a local update, start with a clean working tree:

```sh
git switch develop
git fetch upstream develop
git merge upstream/develop
dotnet test src/NzbDrone.Core.Test/Lidarr.Core.Test.csproj --configuration Debug -p:SolutionDir="$PWD/src/"
git push origin develop
```

Resolve conflicts and run the checks before pushing. Fork migrations currently use versions 82 and 83. If upstream later uses those versions, review and reconcile the schema and migration history before deploying; do not simply renumber migrations that have already run on an existing database.

Source synchronization and application updates are separate. Build and deploy this fork after merging upstream changes. Official Lidarr binaries or container images do not include these features, and an update to an official build can remove them. Keep database backups when testing upgrades.
