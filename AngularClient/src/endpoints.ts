import { environment } from "./environments/environment";

export const endpoints = {
    authorization: `${environment.gatewayUrl}/identity/authorization`,
    registration: `${environment.gatewayUrl}/identity/users`,
    refresh: `${environment.gatewayUrl}/identity/authorization/refresh`,
    users: `${environment.gatewayUrl}/identity/users`,
    authors: `${environment.gatewayUrl}/music/authors`,
    authorByUsername: `${environment.gatewayUrl}/music/authors/username`,
    playlists: `${environment.gatewayUrl}/music/playlists`,
    releases:  `${environment.gatewayUrl}/music/releases`,
    songs: `${environment.gatewayUrl}/music/songs`,
    songSources: `${environment.gatewayUrl}/music/songs/sources`,
    authorInRelease: `${environment.gatewayUrl}/music/releases/author`,
    roles: `${environment.gatewayUrl}/identity/roles`
}