import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { ReleaseService } from "../../services/release.service";
import { ReleaseModel } from "../../models/ReleaseModel";
import { CreateSongModalComponent } from "../create-song-modal/create-song-modal.component";
import { AddSongToReleaseModel } from "../../models/AddSongToReleaseModel";
import { DatePipe, formatDate } from "@angular/common";
import e from "express";
import { UpdateReleaseModel } from "../../models/UpdateRelease";

@Component({
    selector: "edit-release-modal",
    standalone: true,
    templateUrl: "./edit-release-modal.component.html",
    imports: [FormsModule, ReactiveFormsModule, CreateSongModalComponent, DatePipe],
    providers: [ReleaseService]
})
export class EditReleaseModalComponent {
    
    @Input() showModal = false;
    
    @Input() set releaseId(value: string){
        if(value.length > 0) {
            this.loadRelease(value);
        }
    }

    @Output() showModalChange = new EventEmitter<boolean>();
    @Output() releaseChanged = new EventEmitter();

    release = new ReleaseModel();
    showAddSongModal = false;
    errors: string[] = [];

    form: FormGroup;

    constructor (private releaseService: ReleaseService,
                formBuilder: FormBuilder) {
                    this.form = formBuilder.group({
                        name: ["", [Validators.required]],
                        releasedAt: [, [Validators.required]]
                    });
                }


    onCancelEdit(){
        this.showModal = false;
        this.showModalChange.emit(false);
    }

    onDeleteSong(index: number) {
        if (this.release.songs.length <= 1) {
            return;
        }

        let song = this.release.songs[index];

        if (!confirm(`You really want to deleete ${song.title} from ${this.release.name}?`)) {
            return;
        }

        this.releaseService.removeSongFromRelease(this.release.id, song.id)
        .subscribe({
            next: () => {
                this.releaseChanged.emit();
                this.loadRelease(this.release.id);
            },
            error: (error) => {
                console.log(error);
            }
        })
    }

    onAddSong() {
        this.showAddSongModal = true;
    }

    onSongSubmitted(song: AddSongToReleaseModel) {
        this.releaseService.addSongToRelease(this.release.id, song)
            .subscribe({
                next: () => {
                    this.releaseChanged.emit();
                    this.loadRelease(this.release.id);
                },
                error: (error) => {
                    console.log(error);
                }
            })
    }

    onDeleteRelease() {
        if(!confirm(`You really want to delete release ${this.release.name}?`)) {
            return;
        }

        this.releaseService.deleteRelease(this.release.id)
            .subscribe({
                next: () => {
                    this.onCancelEdit();
                    this.releaseChanged.emit();
                },
                error: (error) => {
                    console.log(error);
                }
            })
    }

    onSaveRelease() {
        if (this.form.invalid) {
            return;
        }

        let model = new UpdateReleaseModel();
        model.name = this.form.value.name;
        model.releasedAt = this.form.value.releasedAt;

        this.releaseService.updateRelease(this.release.id, model)
            .subscribe({
                next: (result) => {
                    this.loadRelease(this.release.id);
                    this.releaseChanged.emit();
                    this.onCancelEdit();
                },
                error: (error) => {
                    console.log(error);
                }
            });
    }

    private loadRelease(id: string) {
        this.releaseService.getReleaseDetails(id)
            .subscribe({
                next: (result) => {
                    this.release = result; 
                    this.form.controls['name'].setValue(result.name);
                    this.form.controls['releasedAt'].setValue(formatDate(result.releasedAt,'yyyy-MM-dd','en'));
                    this.form.value.releasedAt = result.releasedAt;
                },
                error: (error) => {
                    console.log(error);
                }
            });
    }
}