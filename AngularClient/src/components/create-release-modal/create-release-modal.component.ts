import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { CreateReleaseModel } from "../../models/CreateReleaseModel";
import { ReleaseService } from "../../services/release.service";
import { Router } from "@angular/router";
import { AddSongToReleaseModel } from "../../models/AddSongToReleaseModel";
import { CreateSongModalComponent } from "../create-song-modal/create-song-modal.component";

@Component({
    selector: "create-release-modal",
    standalone: true,
    templateUrl: "./create-release-modal.component.html",
    imports: [FormsModule, ReactiveFormsModule, CreateSongModalComponent],
    providers: [ReleaseService]
})
export class CreateReleaseModalComponent {
    
    @Input() showModal: boolean = false;
    @Output() showModalChange = new EventEmitter<boolean>();
    @Output() releaseSubmitted = new EventEmitter<CreateReleaseModel>();
    showAddSongModal = false;

    form: FormGroup;
    songs: AddSongToReleaseModel[] = [];

    constructor(formBuilder: FormBuilder,
                private releaseService: ReleaseService){
        this.form = formBuilder.group({
            name: ["", [Validators.required]],
            authors: formBuilder.array([["", [Validators.required]]])
        });
    }

    onCancelNewRelease() {
        this.showModal = false;
        this.showModalChange.emit(false);
    }

    onAddAuthor() {
        (this.form.controls["authors"] as FormArray).push(new FormControl("", [Validators.required]));
    }

    onDeleteAuthor() {
        (this.form.controls["authors"] as FormArray).removeAt(0);
    }

    onDeleteSong(index: number) {
        console.log(index);
        this.songs.splice(index, 1);
    }

    getFormAuthors() {
        return this.form.controls["authors"] as FormArray;
    }

    onAddSong() {
        this.showAddSongModal = true;
    }

    onSongSubmitted(song: AddSongToReleaseModel) {
        this.songs.push(song);
    }

    onSaveRelease() {
        if (this.form.invalid){
            return;
        }

        let releaseModel = new CreateReleaseModel();
        releaseModel.name = this.form.value.name;
        
        (this.form.value.authors as string[]).forEach(author => {
            releaseModel.authorNames.push(author)
        });

        releaseModel.songs = this.songs;
        console.log(releaseModel.songs);

        this.releaseService.createRelease(releaseModel)
        .subscribe({
            next: (result) => {
                this.showAddSongModal = false;
                this.showModalChange.emit(false);
                this.releaseSubmitted.emit(releaseModel);
                this.clear();
            },
            error: (err) => {
                console.log(err);
            }
        })
    }

    clear() {
        this.form.reset();
        this.songs = [];
    }

}