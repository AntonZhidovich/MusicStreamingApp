import { HttpClient } from "@angular/common/http";
import { Injectable, signal } from "@angular/core";
import { SongModel } from "../models/SongModel";
import { Observable } from "rxjs";
import { PageResponse } from "../models/PageResponse";
import { endpoints } from "../endpoints";

@Injectable({
    providedIn: "root"
})
export class SongService {
    songToPlay = signal<SongModel>(new SongModel());

    constructor(private httpClient: HttpClient) {}

    getSongs() : Observable<PageResponse>{
        return this.httpClient.get<PageResponse>(endpoints.songs);
    }

    getSources() {    
        return this.httpClient.get<string[]>(endpoints.songSources);
    }
    
    uploadSource(source: File) {
        let formData = new FormData();
        formData.append("sourceFile", source);

        return this.httpClient.post(endpoints.songSources, formData);
    }

    deleteSource(sourceName: string) {
        let path = `${endpoints.songSources}/${sourceName}`;

        return this.httpClient.delete(path);
    }
}