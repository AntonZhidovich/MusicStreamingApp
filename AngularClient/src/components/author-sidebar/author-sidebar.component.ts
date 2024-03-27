import { Component, EventEmitter, Input, Output } from "@angular/core";
import { AuthorModel } from "../../models/AuthorModel";
import { FormsModule } from "@angular/forms";
import { AuthorService } from "../../services/author.service";
import { AuthService } from "../../services/auth.service";

@Component({
    selector: "author-sidebar",
    standalone: true,
    templateUrl: "./author-sidebar.component.html",
    imports: [FormsModule],
    providers: [AuthorService]
})
export class AuthorSidebarComponent {
    @Input() authorNotFound = false;
    @Input() author = new AuthorModel();
    @Output() onAuthorUpdate = new EventEmitter();
    @Output() onNewRelease = new EventEmitter();

    inputName = "";
    isAddingUserMode = false;
    isCreatingAuthorMode = false;

    errorMessage: string | undefined;

    constructor(private authorService: AuthorService,
                private authService: AuthService) {}

    showAuthor() {
        console.log(this.author);
    }

    onAddClick(){
        this.isAddingUserMode = true;
    }

    onCancelClick(){
        this.errorMessage = undefined;
        this.resetInput();
    }

    onCreateAuthor(){
        this.isCreatingAuthorMode = true;
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
        if (this.isAddingUserMode) {
            this.addUserToAuthor();
        } else {
            this.createAuthor();
        }
    }

    addUserToAuthor() {
        this.authorService.addUserToAuthor(this.author.name, this.inputName)
        .subscribe({
            next: () => {
                this.resetInput;
                this.onAuthorUpdate.emit();
            },
            error: (error) => {
                console.log(error);
                this.errorMessage = error.error.title;
            }
        });
    }

    createAuthor() {
        let userNames = [this.authService.getAuthorizedUsername()];
        this.authorService.createAuthor(this.inputName, userNames)
        .subscribe({
            next: () => {
                this.resetInput();
                this.onAuthorUpdate.emit();
            },
            error: (error) => {
                console.log(error);
                this.errorMessage = error.error.title;
            }
        });
    }

    onDeleteAuthor() {
        if (!confirm(`You sure you want to delete author ${this.author.name}?`)) {
            return;
        }

        this.authorService.deleteAuthor(this.author.name)
            .subscribe({
                next: (result) => {
                    this.authorNotFound = true;
                    this.author = new AuthorModel();
                    this.onAuthorUpdate.emit();
                },
                error: (error) => {
                    console.log(error);
                }
            })
    }

    resetInput() {
        this.isAddingUserMode = false;
        this.isCreatingAuthorMode = false;
        this.inputName = "";
    }
}