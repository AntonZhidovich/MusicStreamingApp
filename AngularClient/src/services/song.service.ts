import { HttpClient } from "@angular/common/http";
import { Injectable, signal } from "@angular/core";
import { environment } from "../environments/environment";
import { SongModel } from "../models/SongModel";
import { Observable } from "rxjs";
import { PageResponse } from "../models/PageResponse";

@Injectable({
    providedIn: "root"
})
export class SongService {

    songUrl = "music/songs";
    songSourcesUrl = "music/songs/sources"
    songToPlay = signal<SongModel>(new SongModel());

    constructor(private httpClient: HttpClient) {}

    getSongs() : Observable<PageResponse>{
        let path = `${environment.gatewayUrl}/${this.songUrl}`;

        return this.httpClient.get<PageResponse>(path);
    }

    getSources() {
        let path = `${environment.gatewayUrl}/${this.songSourcesUrl}`;
        
        return this.httpClient.get<string[]>(path);
    }
    
    uploadSource(source: File) {
        let path = `${environment.gatewayUrl}/${this.songSourcesUrl}`;
        let formData = new FormData();
        formData.append("sourceFile", source);

        return this.httpClient.post(path, formData);
    }

    deleteSource(sourceName: string) {
        let path = `${environment.gatewayUrl}/${this.songSourcesUrl}/${sourceName}`;

        return this.httpClient.delete(path);
    }
}