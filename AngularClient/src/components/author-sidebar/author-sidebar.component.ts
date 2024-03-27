import { Component, EventEmitter, Input, Output } from "@angular/core";
import { AuthorModel } from "../../models/AuthorModel";
import { FormsModule } from "@angular/forms";
import { AuthorService } from "../../services/author.service";

@Component({
    selector: "author-sidebar",
    standalone: true,
    templateUrl: "./author-sidebar.component.html",
    imports: [FormsModule],
    providers: [AuthorService]
})
export class AuthorSidebarComponent {
    @Input() author = new AuthorModel();
    @Output() onAuthorUpdate = new EventEmitter();
    @Output() onNewRelease = new EventEmitter();

    addAuthorInfo = {
        inAddingMode: false,
        userName: ""
    }

    errorMessage: string | undefined;

    constructor(private authorService: AuthorService) {}

    showAuthor() {
        console.log(this.author);
    }

    onAddClick(){
        this.addAuthorInfo.inAddingMode = true;
    }

    onCancelClick(){
        this.errorMessage = undefined;
        this.addAuthorInfo.inAddingMode = false;
        this.addAuthorInfo.userName = "";
    }

    onDeleteClick(userName: string){
        
        if (!confirm(`You sure you want to remove ${userName} from ${this.author.name}?`)){
            return;
        }

        this.authorService.removeUserFromAuthor(this.author.name, userName)
        .subscribe({
            next: () => {this.onAuthorUpdate.emit()},
            error: (error) => {console.log(error)}
        });
    }

    onSaveClick(){
        this.errorMessage = undefined;
        this.authorService.addUserToAuthor(this.author.name, this.addAuthorInfo.userName)
        .subscribe({
            next: () => {
                this.addAuthorInfo.userName = "";
                this.addAuthorInfo.inAddingMode = false;
                this.onAuthorUpdate.emit();
            },
            error: (error) => {
                console.log(error);
                this.errorMessage = error.error.title;
            }
        });
    }

}