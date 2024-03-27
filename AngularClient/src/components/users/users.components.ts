import { Component } from "@angular/core";
import { UserModel } from "../../models/UserModel";
import { UsersService } from "../../services/users.service";
import { DatePipe } from "@angular/common";
import { UserRolesModalComponent } from "../user-roles-modal/user-roles-modal.component";

@Component({
    selector: "users",
    standalone: true,
    templateUrl: "./users.component.html",
    imports: [UserRolesModalComponent, DatePipe]
})
export class UsersComponent {

    users: UserModel[] = [];

    editUserInfo = {
      showModal: false,
      user: new UserModel()
    }

    constructor(private usersService: UsersService) {}

    ngOnInit() {
        this.loadUsers();
    }

    onRolesButton(user: UserModel, event: Event) {
      event.stopPropagation();
      this.editUserInfo.user = user;
      this.editUserInfo.showModal = true;
    }
    
    onDeleteButton(user: UserModel, event: Event) {
      if (!confirm(`You sure you want to delete user ${user.email} with id ${user.id}?`)) {
        return;
      }

      event.stopPropagation();
      this.usersService.deleteUser(user.email)
        .subscribe({
          next: (result) => {
            this.loadUsers();
          },
          error: (error) => {
            console.log(error);
          }});
    }

    loadUsers() {
        this.usersService.getUsers()
          .subscribe({
            next: (result) => {
              this.users = result.users;
            },
          error: (error) => {
            console.log(error);
          }});
      }
}