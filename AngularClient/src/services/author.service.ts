import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthorModel } from "../models/AuthorModel";
import { Observable } from "rxjs";
import { endpoints } from "../endpoints";

@Injectable()
export class AuthorService {

    constructor (private httpClient: HttpClient) {}

    getAuthors(currentPage: number, pageSize: number) : Observable<AuthorModel[]> {
        let params = new HttpParams()
            .set("currentPage", currentPage)
            .set("pageSize", pageSize);

        return this.httpClient.get<AuthorModel[]>(endpoints.authors, {params});
    }

    createAuthor(name: string, userNames: string[]) {
        let model = {
            name: name,
            userNames: userNames
        };

        return this.httpClient.post(endpoints.authors, model);
    }

    deleteAuthor(name: string) {
        return this.httpClient.delete(`${endpoints.authors}/${name}`);
    }

    getAuthorByUserName(username: string) : Observable<AuthorModel> {
        return this.httpClient.get<AuthorModel>(`${endpoints.authorByUsername}/${username}`);
    }

    addUserToAuthor(authorname: string, userName: string) {
        let path = `${endpoints.authors}/${authorname}/artists/${userName}`;
        return this.httpClient.post(path, null);
    }

    removeUserFromAuthor(authorname: string, userName: string) {
        let path = `${endpoints.authors}/${authorname}/artists/${userName}`;
        return this.httpClient.delete(path);
    }
}