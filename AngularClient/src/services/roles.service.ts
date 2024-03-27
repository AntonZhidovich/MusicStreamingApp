import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../environments/environment";
import { Observable } from "rxjs";
import { RoleModel } from "../models/RoleModel";
import { endpoints } from "../endpoints";

@Injectable({
    providedIn: "root"
})
export class RolesService {
    constructor(private httpClient: HttpClient) {}

    getRoles(): Observable<RoleModel[]> {
        return this.httpClient.get<RoleModel[]>(endpoints.roles);
    }

    deleteRole(name: string) {
        let path = `${endpoints.roles}/${name}`

        return this.httpClient.delete(path);
    }

    createRole(role: RoleModel) {
        return this.httpClient.post(endpoints.roles, role);
    }
}