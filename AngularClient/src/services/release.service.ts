import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PageResponse } from "../models/PageResponse";
import { environment } from "../environments/environment";
import { CreateReleaseModel } from "../models/CreateReleaseModel";
import { ReleaseModel } from "../models/ReleaseModel";
import { AddSongToReleaseModel } from "../models/AddSongToReleaseModel";
import { UpdateReleaseModel } from "../models/UpdateRelease";

@Injectable()
export class ReleaseService {
    
    releaseUrl = "music/releases";

    constructor(private httpClient: HttpClient) {}

    getReleases(currentPage: number, pageSize: number) : Observable<PageResponse> {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}`;
        let params = new HttpParams()
        .set("currentPage", currentPage)
        .set("pageSize", pageSize);

        return this.httpClient.get<PageResponse>(path, {params});
    }

    getReleasesFromAuthor(authorName: string, currentPage: number, pageSize: number) : Observable<PageResponse> {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}/author/${authorName}`;
        let params = new HttpParams()
            .set("currentPage", currentPage)
            .set("pageSize", pageSize);
        
        return this.httpClient.get<PageResponse>(path, {params});
    }

    createRelease(release: CreateReleaseModel) {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}`;
        
        return this.httpClient.post(path, release);
    }

    updateRelease(id: string, release: UpdateReleaseModel) {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}/${id}`;

        return this.httpClient.put(path, release);
    }

    deleteRelease(id: string) {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}/${id}`;

        return this.httpClient.delete(path);
    }

    getReleaseDetails(id: string) : Observable<ReleaseModel> {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}/${id}`;

        return this.httpClient.get<ReleaseModel>(path);
    }

    addSongToRelease(releaseId: string, song: AddSongToReleaseModel){
        let path = `${environment.gatewayUrl}/${this.releaseUrl}/${releaseId}/songs`;

        return this.httpClient.post(path, song);
    }

    removeSongFromRelease(releaseId: string, songId: string) {
        let path = `${environment.gatewayUrl}/${this.releaseUrl}/${releaseId}/songs/${songId}`;

        return this.httpClient.delete(path);
    }
}
