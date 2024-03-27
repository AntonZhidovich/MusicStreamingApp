import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../environments/environment";
import { Observable } from "rxjs";
import { RoleModel } from "../models/RoleModel";

@Injectable({
    providedIn: "root"
})
export class RolesService {
    rolesUrl = `${environment.gatewayUrl}/identity/roles`

    constructor(private httpClient: HttpClient) {}

    getRoles(): Observable<RoleModel[]> {
        let path = this.rolesUrl;

        return this.httpClient.get<RoleModel[]>(path);
    }

    deleteRole(name: string) {
        let path = `${this.rolesUrl}/${name}`

        return this.httpClient.delete(path);
    }

    createRole(role: RoleModel) {
        return this.httpClient.post(this.rolesUrl, role);
    }
}