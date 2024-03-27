import { Component, Injector, Input, effect } from "@angular/core";
import { SongModel } from "../../models/SongModel";
import { PlayerService } from "../../services/player.service";
import { FormsModule } from "@angular/forms";


@Component({
    selector: "player",
    standalone: true,
    templateUrl: "./player.component.html",
    imports: [FormsModule]
})
export class PlayerComponent {
    
    showPlayer = false;
    currentSong: SongModel = new SongModel();
    player: HTMLAudioElement;
    currentTime = 0;
    @Input() isPlaying = false; 

    get currentSongTime () {
        return this.getTimeString(this.player.currentTime);
    }

    get songDuration () {
        return this.getTimeString(this.player.duration);
    }

    constructor(public playerService: PlayerService) {

        this.player = this.playerService.audioElement;

        effect(() => {
            this.currentSong = this.playerService.currentSong();
            if(this.currentSong.sourceName.length > 0) {
                this.showPlayer = true;
                this.player.src = "https://localhost:8080/music/songs/sources/"+ this.currentSong.sourceName;
            }
            this.onPlayButton();
        });
    }

    ngOnInit() {
        this.player.addEventListener("timeupdate", (event) => this.onTimeUpdate(event));

        if (this.isPlaying) {
            this.player.play();
        }
    }

    onPlayButton() {
        this.isPlaying = true; 
        this.player.play();
    }

    onPauseButton() {
        this.isPlaying = false; 
        this.player.pause();
    }

    onMuteButton() {
        this.player.muted = !this.player.muted;
    }

    onTimeUpdate(event: Event) {
        this.currentTime = 100 * this.player.currentTime / this.player.duration;
    }

    onNextButton() {
        this.playerService.onNextSong();
    }

    onPreviousButton() {
        this.playerService.onPreviousSong();
    }

    onVolumeInput(volume: string) {
        let value = parseInt(volume);
        this.playerService.setVolume(value / 100);
    }

    getTimeString(time: number) {
        const hours = Math.floor(time / 3600);
        const minutes = Math.floor((time - (hours * 3600)) / 60);
        const seconds = Math.floor(time - ((hours * 3600) + (minutes * 60)));
        const minutesString  = minutes < 10 ? "0" + minutes : "" + minutes;
        const secondsString = seconds < 10 ? "0" + seconds : "" + seconds;
        const result = minutesString + ":" + secondsString;
        return result
    }
    
    omTimeLineClick(event: MouseEvent, progressBar: HTMLElement) {
        this.currentTime = 100*event.offsetX / progressBar.clientWidth;
        this.player.currentTime = this.currentTime*this.player.duration /100
    }

}