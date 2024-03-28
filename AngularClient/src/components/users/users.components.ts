import { Component } from "@angular/core";
import { UserModel } from "../../models/UserModel";
import { UsersService } from "../../services/users.service";
import { DatePipe } from "@angular/common";
import { UserRolesModalComponent } from "../user-roles-modal/user-roles-modal.component";
import { InfiniteScrollDirective } from "../../InfiniteScrollDirective";

@Component({
    selector: "users",
    standalone: true,
    templateUrl: "./users.component.html",
    imports: [InfiniteScrollDirective, UserRolesModalComponent, DatePipe]
})
export class UsersComponent {

    users: UserModel[] = [];

    editUserInfo = {
      showModal: false,
      user: new UserModel()
    }

    pageInfo = {
      currentPage: 0,
      pageSize: 10,
      pagesCount: Number.MAX_VALUE
    }
    isLoadingPage = false;

    constructor(private usersService: UsersService) {}

    ngOnInit() {
        this.loadNextPage();
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
            this.loadNextPage();
          },
          error: (error) => {
            console.log(error);
          }});
    }

    onLoadElements() {
      if (!this.isLoadingPage){
        if (this.pageInfo.currentPage < this.pageInfo.pagesCount) {
          this.loadNextPage();
        }
      }
    }

    loadNextPage() {
      ++this.pageInfo.currentPage;
      this.isLoadingPage = true;
      this.usersService.getUsers(this.pageInfo.currentPage, this.pageInfo.pageSize)
        .subscribe({
          next: (result) => {
            this.users = this.users.concat(result.users);
            this.pageInfo.pagesCount = result.pagesCount;
            this.isLoadingPage = false;
          },
          error: (error) => {
            console.log(error);
            this.isLoadingPage = false;
          }});
      }
      
}