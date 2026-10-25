# Lidarr Primetime

This fork extends [upstream Lidarr](https://github.com/Lidarr/Lidarr) with individual-song downloads. Choose one song or several tracks from an album; they do not need to have been released as singles.

## Changes compared with upstream Lidarr

* **Individual-song searches:** search for the artist and song title first, then fall back to releases containing the same recording, including albums, EPs, compilations, and discographies. Accepted song downloads take priority over larger releases.
* **Selective imports:** keep only the requested MusicBrainz recordings when an album is downloaded as a fallback. Completion requires every requested recording. Whole-album requests retain their normal import behavior.
* **Separate Albums and Songs views:** browse either view from an artist page, with a direct Songs link and selection preserved when switching tabs. Opening a view does not change monitoring.
* **Simpler song selection:** click rows or checkboxes, Shift-click a range, use keyboard selection, filter by song or album, and download the checked songs in one action. One-time downloads do not change artist or album monitoring; existing Song Mode artists also have a separate save-monitoring action.
* **Clear library availability:** downloaded songs have a green row and an explicit Downloaded badge, with downloaded/missing counts and status filters. Availability follows the recording across album appearances, and downloaded songs remain selectable for upgrades.
* **External popularity data:** show and sort by a song's position in the artist's Deezer top tracks, with source links and checks to distinguish similarly named artists and alternate recordings. This is a popularity ranking, not a community rating or a song-quality score.
* **Wanted songs in the queue:** selective downloads show the requested song titles alongside the album, using the intent saved with that download rather than the current checklist.
* **More reliable transfer and import tracking:** preserve requested recording IDs through persisted history, restarts, external completed-download scans, and selective import. Handle camel-cased history fields and distinguish full-album downloads from song requests.
* **Song APIs and regression coverage:** expose track-search capabilities, recording-aware search and import behavior, and focused backend, API, and frontend tests for the song workflow.

Song availability still depends on the configured indexers and release metadata. This fork does not split a single audio file that contains multiple recordings. See [Song Mode and fork maintenance](FORK.md) for usage, API details, and import behavior.

**Upstream synchronization is disabled.** The sync workflow has no scheduled trigger, and its GitHub Action is disabled. The fork can still be updated with a reviewed manual upstream merge.

## Upstream Lidarr

[![Build Status](https://dev.azure.com/Lidarr/Lidarr/_apis/build/status/lidarr.Lidarr?branchName=develop)](https://dev.azure.com/Lidarr/Lidarr/_build/latest?definitionId=1&branchName=develop)
[![Translation status](https://translate.servarr.com/widget/servarr/lidarr/svg-badge.svg)](https://translate.servarr.com/engage/servarr/?utm_source=widget)
[![Docker Pulls](https://img.shields.io/docker/pulls/linuxserver/lidarr.svg)](https://wiki.servarr.com/lidarr/installation#docker)
![Github Downloads](https://img.shields.io/github/downloads/lidarr/lidarr/total.svg)
[![Backers on Open Collective](https://opencollective.com/lidarr/backers/badge.svg)](#backers) 
[![Sponsors on Open Collective](https://opencollective.com/lidarr/sponsors/badge.svg)](#sponsors)

Lidarr is a music collection manager for Usenet and BitTorrent users. It can monitor multiple RSS feeds for new tracks from your favorite artists and will grab, sort and rename them. It can also be configured to automatically upgrade the quality of files already downloaded when a better quality format becomes available.

> [!WARNING]
> NOTICE - The Lidarr Metadata Server is recovering and rebuilding the cache which is impacting adding artists, library imports, etc. Please follow [GHI 5498](https://github.com/Lidarr/Lidarr/issues/5498) or see Discord for details.

## Major Features Include:

* Support for major platforms: Windows, Linux, macOS, Raspberry Pi, etc.
* Automatically detects new tracks.
* Can scan your existing library and download any missing tracks.
* Can watch for better quality of the tracks you already have and do an automatic upgrade.
* Automatic failed download handling will try another release if one fails
* Manual search so you can pick any release or to see why a release was not downloaded automatically
* Fully configurable track renaming
* Full integration with SABnzbd and NZBGet
* Full integration with Kodi, Plex (notification, library update, metadata)
* Full support for specials and multi-album releases
* And a beautiful UI

## Support

Note: GitHub Issues are for Bugs and Feature Requests Only

[![Discord](https://img.shields.io/badge/discord-chat-7289DA.svg?maxAge=60)](https://lidarr.audio/discord)
[![GitHub - Bugs and Feature Requests Only](https://img.shields.io/badge/github-issues-red.svg?maxAge=60)](https://github.com/Lidarr/Lidarr/issues)
[![Wiki](https://img.shields.io/badge/servarr-wiki-181717.svg?maxAge=60)](https://wiki.servarr.com/lidarr)

## Contributors

This project exists thanks to all the people who contribute. [Contribute](CONTRIBUTING.md).
<a href="https://github.com/lidarr/Lidarr/graphs/contributors"><img src="https://opencollective.com/lidarr/contributors.svg?width=890&button=false" /></a>

## Backers

Thank you to all our backers! 🙏 [Become a backer](https://opencollective.com/Lidarr#backer)

<img src="https://opencollective.com/Lidarr/backers.svg?width=890"></a>

## Sponsors

Support this project by becoming a sponsor. Your logo will show up here with a link to your website. [Become a sponsor](https://opencollective.com/Lidarr#sponsor)

<img src="https://opencollective.com/Lidarr/sponsors.svg?width=890"></a>

## Mega Sponsors
<img src="https://opencollective.com/Lidarr/tiers/mega-sponsor.svg?width=890"></a>

## JetBrains
Thank you to [<img src="/Logo/jetbrains.svg" alt="JetBrains" width="32"> JetBrains](http://www.jetbrains.com/) for providing us with free licenses to their great tools.
 
* [<img src="/Logo/resharper.svg" alt="ReSharper" width="32"> ReSharper](http://www.jetbrains.com/resharper/)
* [<img src="/Logo/webstorm.svg" alt="WebStorm" width="32"> WebStorm](http://www.jetbrains.com/webstorm/)
* [<img src="/Logo/rider.svg" alt="Rider" width="32"> Rider](http://www.jetbrains.com/rider/)
* [<img src="/Logo/dottrace.svg" alt="dotTrace" width="32"> dotTrace](http://www.jetbrains.com/dottrace/)

## DigitalOcean

This project is also supported by DigitalOcean
<p>
  <a href="https://www.digitalocean.com/">
    <img src="https://opensource.nyc3.cdn.digitaloceanspaces.com/attribution/assets/SVG/DO_Logo_horizontal_blue.svg" width="201px">
  </a>
</p>

### License

* [GNU GPL v3](http://www.gnu.org/licenses/gpl.html)
* Copyright 2010-2021
