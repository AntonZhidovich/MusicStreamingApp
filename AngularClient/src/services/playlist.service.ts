import { Injectable, SkipSelf } from "@angular/core";
import { environment } from "../environments/environment";
import { PlaylistModel } from "../models/PlaylistModel";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { PlaylistWithSongsModel } from "../models/PlaylisWithSongsModel";

@Injectable({
    providedIn: "root"
})
export class PlaylistService {
    
    private playlistUrl = "music/playlists";

    constructor (private httpClient: HttpClient) {}

    delete(id: string){
        let path = `${environment.gatewayUrl}/${this.playlistUrl}/${id}`;

        return this.httpClient.delete(path);
    }

    create(name: string) {
        let path = `${environment.gatewayUrl}/${this.playlistUrl}`;

        var model = {
            "name" : name
        };

        return this.httpClient.post(path, model);
    }

    update(newName: string, id: string) {
        let path = `${environment.gatewayUrl}/${this.playlistUrl}/${id}`;

        var model = {
            "name" : newName
        };

        return this.httpClient.put(path, model);
    }

    getPlaylists() : Observable<PlaylistModel[]> {
        let path = `${environment.gatewayUrl}/${this.playlistUrl}`;
        
        return this.httpClient.get<PlaylistModel[]>(path);
    }

    addSongToPlaylist(playlistId: string, songId: string) {
        let path = `${environment.gatewayUrl}/${this.playlistUrl}/${playlistId}/songs`;

        return this.httpClient.post(path, {
            songId: songId
        });
    }

    deleteSongFromPlaylist(playlistId: string, songId: string) {
        let path = `${environment.gatewayUrl}/${this.playlistUrl}/${playlistId}/songs/${songId}`;

        return this.httpClient.delete(path);
    }

    getPlaylistWithSongs(playlistId : string): Observable<PlaylistWithSongsModel>  {
        let path = `${environment.gatewayUrl}/${this.playlistUrl}/${playlistId}/songs`;

        return this.httpClient.get<PlaylistWithSongsModel>(path);
    }
}