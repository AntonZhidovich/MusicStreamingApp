import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PageResponse } from "../models/PageResponse";
import { environment } from "../environments/environment";
import { CreateReleaseModel } from "../models/CreateReleaseModel";
import { ReleaseModel } from "../models/ReleaseModel";
import { AddSongToReleaseModel } from "../models/AddSongToReleaseModel";
import { UpdateReleaseModel } from "../models/UpdateRelease";
import { endpoints } from "../endpoints";

@Injectable()
export class ReleaseService {
    constructor(private httpClient: HttpClient) {}

    getReleases(currentPage: number, pageSize: number) : Observable<PageResponse> {
        let params = new HttpParams()
        .set("currentPage", currentPage)
        .set("pageSize", pageSize);

        return this.httpClient.get<PageResponse>(endpoints.releases, {params});
    }

    getReleasesFromAuthor(authorName: string, currentPage: number, pageSize: number) : Observable<PageResponse> {
        let params = new HttpParams()
            .set("currentPage", currentPage)
            .set("pageSize", pageSize);
        
        return this.httpClient.get<PageResponse>(`${endpoints.authorInRelease}/${authorName}`, {params});
    }

    createRelease(release: CreateReleaseModel) { 
        return this.httpClient.post(endpoints.releases, release);
    }

    updateRelease(id: string, release: UpdateReleaseModel) {
        return this.httpClient.put(`${endpoints.releases}/${id}`, release);
    }

    deleteRelease(id: string) {
        return this.httpClient.delete(`${endpoints.releases}/${id}`);
    }

    getReleaseDetails(id: string) : Observable<ReleaseModel> {
        return this.httpClient.get<ReleaseModel>(`${endpoints.releases}/${id}`);
    }

    addSongToRelease(releaseId: string, song: AddSongToReleaseModel){
        let path = `${endpoints.releases}/${releaseId}/songs`;

        return this.httpClient.post(path, song);
    }

    removeSongFromRelease(releaseId: string, songId: string) {
        let path = `${endpoints.releases}/${releaseId}/songs/${songId}`;

        return this.httpClient.delete(path);
    }
}
