import { Component, EventEmitter, Input, Output } from "@angular/core";
import { AddSongToReleaseModel } from "../../models/AddSongToReleaseModel";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { SongService } from "../../services/song.service";

@Component({
    selector: "create-song-modal",
    standalone: true,
    templateUrl: "./create-song-modal.component.html",
    imports: [FormsModule, ReactiveFormsModule],
    providers: [SongService]
})
export class CreateSongModalComponent {
    @Output() showModalChange = new EventEmitter<boolean>()
    @Output() songSubmitted = new EventEmitter<AddSongToReleaseModel>();
    private _showModal: boolean = false;
    
    @Input() set showModal(value: boolean){
        this._showModal = value;
        
        if (this._showModal) {
            this.loadSources();
        }
    }
    
    get showModal() {
        return this._showModal;        
    }
    
    song = new AddSongToReleaseModel();
    form: FormGroup;
    genres: string[] = [];
    songSources: string[] = [];
    selectedSource = "";
    sourceFile: File | undefined;

    constructor(formBuilder: FormBuilder,
                private songService: SongService) {
        this.form = formBuilder.group({
            title: ["", [Validators.required]],
            genre: [""],
            duration: ["", [Validators.required]]
        });
    }

    onAddGenre() {
        if (this.form.value.genre.length > 0) {
            this.genres.push(this.form.value.genre);
            this.form.controls['genre'].reset();
        }
    }

    onClearGenres() {
        this.genres = this.genres.slice(0, this.genres.length-1);
    }

    onCancelNewSong() {
        this.resetForm();
        this._showModal = false;
        this.showModalChange.emit(false);
    }

    loadSources() {
        this.songService.getSources().subscribe({
            next: (result: string[]) => {
                this.songSources = result;
            },
            error: (err) => {
                console.log(err);
                this.songSources = [];
            }
        })
    }

    onSourceSelected(event: any) {
        let file = event.target.files[0]; 
        
        if (file) {
            this.sourceFile = file;
        } 
    }

    onUploadSource(){
        if (this.sourceFile) {
            this.songService.uploadSource(this.sourceFile)
            .subscribe({
                next: (result) => {
                    this.loadSources();
                },
                error: (err) => {
                    console.log(err);
                }
            });
        }
    }

    onDeleteSource() {
        let sourceToDelete: string = this.selectedSource;
        if (sourceToDelete) {
            sourceToDelete = sourceToDelete.split('/')[1];
            this.songService.deleteSource(sourceToDelete)
            .subscribe({
                next: (result) => {
                    this.loadSources();
                },
                error: (err) => {
                    console.log(err);
                }
            });
        }
    }
        
    onSaveSong() {
        if (!this.formIsValid()){
            return;
        }
        
        let songModel = new AddSongToReleaseModel();
        songModel.title = this.form.value.title;
        songModel.durationMinutes = this.form.value.duration;
        songModel.genres = this.genres;
        songModel.sourceName = this.selectedSource;
        
        this.songSubmitted.emit(songModel);
        this.resetForm();
        this._showModal = false;
        this.showModalChange.emit(false);
    }

    private resetForm() {
        this.form.reset();
        this.songSources = [];
        this.genres = [];
    }

    private formIsValid() {
        return this.form.valid && this.selectedSource.length > 0 && this.genres.length > 0;
    }
} 