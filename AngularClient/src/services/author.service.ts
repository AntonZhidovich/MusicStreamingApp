import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../environments/environment";
import { AuthorModel } from "../models/AuthorModel";
import { Observable } from "rxjs";

@Injectable()
export class AuthorService {

    authorsUrl = "music/authors";

    constructor (private httpClient: HttpClient) {}

    getAuthors(currentPage: number, pageSize: number) : Observable<AuthorModel[]> {
        let path = `${environment.gatewayUrl}/${this.authorsUrl}`;
        let params = new HttpParams()
            .set("currentPage", currentPage)
            .set("pageSize", pageSize);

        return this.httpClient.get<AuthorModel[]>(path, {params});
    }

    createAuthor(name: string, userNames: string[]) {
        let model = {
            name: name,
            userNames: userNames
        };
        
        let path = `${environment.gatewayUrl}/${this.authorsUrl}`;

        return this.httpClient.post(path, model);
    }

    deleteAuthor(name: string) {
        let path =  `${environment.gatewayUrl}/${this.authorsUrl}/${name}`;

        return this.httpClient.delete(path);
    }

    getAuthorByUserName(username: string) : Observable<AuthorModel> {
        let path = `${environment.gatewayUrl}/${this.authorsUrl}/username/${username}`;
        return this.httpClient.get<AuthorModel>(path);
    }

    addUserToAuthor(authorname: string, userName: string) {
        let path = `${environment.gatewayUrl}/${this.authorsUrl}/${authorname}/artists/${userName}`;
        return this.httpClient.post(path, null);
    }

    removeUserFromAuthor(authorname: string, userName: string) {
        let path = `${environment.gatewayUrl}/${this.authorsUrl}/${authorname}/artists/${userName}`;
        return this.httpClient.delete(path);
    }
}