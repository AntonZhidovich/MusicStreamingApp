import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../environments/environment";
import { PageResponse } from "../models/PageResponse";
import { Observable } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class UsersService {
    usersUrl = `${environment.gatewayUrl}/identity/users`;

    constructor (private httpClient: HttpClient) {}

    getUsers(): Observable<any> {
        return this.httpClient.get<PageResponse>(this.usersUrl);
    }

    getUserRoles(email: string): Observable<string[]> {
        let path = `${this.usersUrl}/${email}/roles`;

        return this.httpClient.get<string[]>(path);
    }

    addUserToRole(email: string, roleName: string) {
        let path = `${this.usersUrl}/${email}/roles/${roleName}`;

        return this.httpClient.post(path, {});
    }

    removeUserFromRole(email: string, roleName: string) {
        let path = `${this.usersUrl}/${email}/roles/${roleName}`;

        return this.httpClient.delete(path);
    }

    deleteUser(email: string) {
        let path = `${this.usersUrl}/${email}`;

        return this.httpClient.delete(path);
    }
}