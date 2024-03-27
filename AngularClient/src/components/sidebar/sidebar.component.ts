import { Component, HostListener } from "@angular/core";
import { PlaylistModel } from "../../models/PlaylistModel";
import { PlaylistService } from "../../services/playlist.service";
import { FormsModule } from "@angular/forms";

@Component({
    selector: "sidebar",
    standalone: true,
    templateUrl: "./sidebar.component.html",
    imports: [FormsModule],
    providers: [PlaylistService]
})
export class SidebarComponent {

    height: number = 0;
    width:number = 450;
    playlists: PlaylistModel[] = [];
    errorMessage: string | undefined;

    newPlaylistInfo = {
        creatingMode: false,
        name: ""
    }

    resizeInfo = {
        isPressed: false,
        minWidth: 450,
        maxWidth: window.outerWidth*0.6
    }

    constructor(private playlistService : PlaylistService){}

    ngOnInit() : void{
        this.loadPlaylists();
    }

    onDeleteClick(id: string, name: string){
        
        if (!confirm(`You sure you want to delete playlist ${name}?`)){
            return;
        }

        this.playlistService.delete(id)
        .subscribe({
            next: () => {this.loadPlaylists();},
            error: (error) => {console.log(error)}
        });
    }

    onAddClick(){
        this.newPlaylistInfo.creatingMode = true;
    }

    onSaveClick(){
        this.errorMessage = undefined;
        this.playlistService.create(this.newPlaylistInfo.name)
        .subscribe({
            next: () => {
                this.newPlaylistInfo.name = "";
                this.newPlaylistInfo.creatingMode = false;
                this.loadPlaylists();
            },
            error: (error) => {
                console.log(error);
                this.errorMessage = error.error.title;
            }
        });

        this.loadPlaylists();
    }

    onCancelClick(){
        this.errorMessage = undefined;
        this.newPlaylistInfo.creatingMode = false;
        this.newPlaylistInfo.name = "";
    }

    OnResizerMouseDown(event: MouseEvent) {
        this.resizeInfo.isPressed = true;
    }

    private loadPlaylists() {
        this.playlistService.getPlaylists()
        .subscribe((result: PlaylistModel[]) => {
            this.playlists = result;
        });
    }

    @HostListener("window:mousemove", ["$event"])
    OnMouseMove(event: MouseEvent){
        if (!this.resizeInfo.isPressed){
            return;
        }

        let mouseX = event.clientX;

        if (mouseX >= this.resizeInfo.minWidth && mouseX <= this.resizeInfo.maxWidth){
            this.width = mouseX;
        }
    }

    @HostListener("window:mouseup", ["$event"])
    OnMouseUp(event: MouseEvent){
        this.resizeInfo.isPressed = false;
    }
}