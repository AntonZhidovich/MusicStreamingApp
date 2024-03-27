import { Component, EventEmitter, Input, Output } from "@angular/core";
import { UsersService } from "../../services/users.service";
import { UserModel } from "../../models/UserModel";
import { FormsModule } from "@angular/forms";
import { RolesService } from "../../services/roles.service";
import { RoleModel } from "../../models/RoleModel";

@Component({
    selector: "user-roles-modal",
    standalone: true,
    templateUrl: "./user-roles-modal.component.html",
    imports: [FormsModule]
})
export class UserRolesModalComponent {

    @Input() user = new UserModel();

    @Input() set showModal(value: boolean) {
        this._showModal = value;

        if (this._showModal){
            this.loadUserRoles();
            this.loadRoles();
        }
    }

    @Output() showModalChange = new EventEmitter<boolean>();

    get showModal() {
        return this._showModal;
    }

    userRoles: string[] = [];
    roles: RoleModel[] = [];
    selectedRole = "";
    private _showModal = false;

    constructor(private usersService: UsersService,
                private roleService: RolesService) {}


    onSave() {
        if (this.selectedRole.length == 0){
            return;
        }

        this.usersService.addUserToRole(this.user.email, this.selectedRole)
        .subscribe({
            next: (result) => {
                this.loadUserRoles();
            },
            error: (error) => {
                console.log(error);
            }
        });
    }
         
    onRemove(role: string) {
        this.usersService.removeUserFromRole(this.user.email, role)
        .subscribe({
            next: (result) => {
                this.loadUserRoles();
            },
            error: (error) => {
                console.log(error);
            }
        });
    }

    onCancel() {
        this.showModal = false;
        this.showModalChange.emit(false);
    }

    loadUserRoles() {
        this.usersService.getUserRoles(this.user.email)
            .subscribe({
                next: (result) => {
                    this.userRoles = result;
                },
                error: (error) => {
                    console.log(error);
                }
            });
    }

    loadRoles() {
        this.roleService.getRoles()
          .subscribe({
            next: (result) => {
              this.roles = result;
            },
          error: (error) => {
            console.log(error);
          }});
      }
}