import { Component } from "@angular/core";
import { NavbarComponent } from "../navbar/navbar.component";
import { AuthorModel } from "../../models/AuthorModel";
import { AuthService } from "../../services/auth.service";
import { AuthorService } from "../../services/author.service";
import { CreatorReleaseListComponent } from "../creator-release-list/creator-release-list.component";
import { AuthorSidebarComponent } from "../author-sidebar/author-sidebar.component";

@Component({
    selector: "creator",
    standalone: true,
    templateUrl: "./creator.component.html",
    imports: [NavbarComponent, CreatorReleaseListComponent, AuthorSidebarComponent],
    providers: [AuthorService]
})
export class CreatorComponent {
    authorNotFound = false;
    currentAuthor = new AuthorModel(); 

    constructor(private authService: AuthService,
                private authorService: AuthorService) {}

    ngOnInit(){
        this.loadCurrentUserAuthor();  
    }

    loadCurrentUserAuthor() {
        let currentUsername = this.authService.getAuthorizedUsername();

        this.authorService.getAuthorByUserName(currentUsername)
        .subscribe({
            next: (result: AuthorModel) => {
                this.currentAuthor = result;
                console.log(result);
                this.authorNotFound = false;
            },
            error: (error) => {
                console.log(error);
                this.authorNotFound = true;
            }
        })
    }
}