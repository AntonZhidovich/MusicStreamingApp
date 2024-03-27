import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PageResponse } from "../models/PageResponse";
import { Observable } from "rxjs";
import { endpoints } from "../endpoints";

@Injectable({
    providedIn: "root"
})
export class UsersService {
    constructor (private httpClient: HttpClient) {}

    getUsers(): Observable<any> {
        return this.httpClient.get<PageResponse>(endpoints.users);
    }

    getUserRoles(email: string): Observable<string[]> {
        let path = `${endpoints.users}/${email}/roles`;

        return this.httpClient.get<string[]>(path);
    }

    addUserToRole(email: string, roleName: string) {
        let path = `${endpoints.users}/${email}/roles/${roleName}`;

        return this.httpClient.post(path, {});
    }

    removeUserFromRole(email: string, roleName: string) {
        let path = `${endpoints.users}/${email}/roles/${roleName}`;

        return this.httpClient.delete(path);
    }

    deleteUser(email: string) {
        let path = `${endpoints.users}/${email}`;

        return this.httpClient.delete(path);
    }
}