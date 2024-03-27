import { Component, Input } from "@angular/core";
import { AuthorModel } from "../../models/AuthorModel";
import { ReleaseService } from "../../services/release.service";
import { ReleaseModel } from "../../models/ReleaseModel";
import { DatePipe } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { AuthorService } from "../../services/author.service";
import { AddSongToReleaseModel } from "../../models/AddSongToReleaseModel";
import { CreateReleaseModalComponent } from "../create-release-modal/create-release-modal.component";
import { CreateSongModalComponent } from "../create-song-modal/create-song-modal.component";
import { ReleaseItemComponent } from "../release-item/release-item.component";
import { EditReleaseModalComponent } from "../edit-release-modal/edit-release-modal-component";

@Component ({
    selector: "creator-release-list",
    standalone: true,
    templateUrl: "./creator-release-list.component.html",
    imports: [
        DatePipe, 
        FormsModule, 
        ReactiveFormsModule, 
        CreateReleaseModalComponent, 
        ReleaseItemComponent,
        CreateSongModalComponent,
        EditReleaseModalComponent],
    providers: [ReleaseService, AuthService, AuthorService]
})
export class CreatorReleaseListComponent {

    releases: ReleaseModel[] = [];
    releaseToEdit = new ReleaseModel();
    songs: AddSongToReleaseModel[] = [];
    isCreatingRelease = false;
    isAddingSong = false;
    isEditingRelease = false;

    private author: AuthorModel = new AuthorModel();
    
    @Input() set currentAuthor(value: AuthorModel) {        
        if (value.name.length > 0) {
            this.author = value;
            this.loadReleases();
        }
    }
    
    constructor(private releaseService: ReleaseService) {}

    loadReleases() {
        this.releaseService.getReleasesFromAuthor(this.author!.name, 1, 20)
                .subscribe({
                    next: (result) => {
                    this.releases = <ReleaseModel[]>result.items;
                },
                error: (error) => {
                    console.log(error);
                }
            });
        }

    onButtonNewRelease() {
        this.isCreatingRelease = true;
    }

    onItemClick(release: ReleaseModel){
        this.releaseToEdit = release;
        this.isEditingRelease = true;
    }

    onReleaseSubmitted() {
        this.loadReleases();
    }
}