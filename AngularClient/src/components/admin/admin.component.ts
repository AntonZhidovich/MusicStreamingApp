import { Component } from "@angular/core";
import { RolesSidebarComponent } from "../roles-sidebar/roles-sidebar.component";
import { UsersComponent } from "../users/users.components";

@Component({
    selector: "admin",
    standalone: true,
    templateUrl: "./admin.component.html",
    imports: [RolesSidebarComponent, UsersComponent]
})
export class AdminComponent {}