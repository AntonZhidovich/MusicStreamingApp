import { Injectable } from "@angular/core";
import { PlaylistModel } from "../models/PlaylistModel";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { PlaylistWithSongsModel } from "../models/PlaylisWithSongsModel";
import { endpoints } from "../endpoints";

@Injectable({
    providedIn: "root"
})
export class PlaylistService {
    constructor (private httpClient: HttpClient) {}

    delete(id: string){
        return this.httpClient.delete(`${endpoints.playlists}/${id}`);
    }

    create(name: string) {
        var model = {
            "name" : name
        };

        return this.httpClient.post(endpoints.playlists, model);
    }

    update(newName: string, id: string) {
        var model = {
            "name" : newName
        };

        return this.httpClient.put(`${endpoints.playlists}/${id}`, model);
    }

    getPlaylists() : Observable<PlaylistModel[]> {       
        return this.httpClient.get<PlaylistModel[]>(endpoints.playlists);
    }

    addSongToPlaylist(playlistId: string, songId: string) {
        let path = `${endpoints.playlists}/${playlistId}/songs`;

        return this.httpClient.post(path, {
            songId: songId
        });
    }

    deleteSongFromPlaylist(playlistId: string, songId: string) {
        let path = `${endpoints.playlists}/${playlistId}/songs/${songId}`;

        return this.httpClient.delete(path);
    }

    getPlaylistWithSongs(playlistId : string): Observable<PlaylistWithSongsModel>  {
        let path =`${endpoints.playlists}/${playlistId}/songs`;

        return this.httpClient.get<PlaylistWithSongsModel>(path);
    }
}