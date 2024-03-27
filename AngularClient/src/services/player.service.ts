import { Injectable, signal } from '@angular/core';
import { SongService } from './song.service';
import { SongModel } from '../models/SongModel';
import { EventEmitter } from 'stream';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  readonly songStartDurationSeconds = 3;

  currentSong = signal<SongModel>(new SongModel());
  
  playlist: SongModel[] = [];
  playbackMode: RepeatPlayback = RepeatPlayback.RepeatAll;
  currentIndex: number = 0;
  audioElement: HTMLAudioElement;

  constructor(private songService: SongService) {
    this.audioElement = new Audio();
    this.audioElement.addEventListener("ended", (event) => this.selectNextSong());
    this.audioElement.addEventListener("error", (event) => this.handleError(event));
  }

  onNextSong() {
    this.audioElement.currentTime = this.audioElement.duration;
  }

  setCurrentSong(index: number) {
    this.currentIndex = index;
    this.currentSong.set(this.playlist[index]);
  } 

  onPreviousSong() {
    let currentTime = this.audioElement.currentTime;
    if (currentTime < this.songStartDurationSeconds) {
      this.selectPreviousSong();
    } else {
      this.audioElement.currentTime = 0;
    }
  }

  setRepeatPlayBackMode(mode: RepeatPlayback) {
    this.playbackMode = mode;
  }

  setPlaylist(songs: SongModel[], index: number = 0) {
    this.playlist = Array.from(songs);
    this.currentIndex = index;
    this.currentSong.set(this.playlist[index]);
  }

  selectPreviousSong() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
      this.currentSong.set(this.playlist[this.currentIndex]);
    }
  }

  setVolume(volume: number) {
    this.audioElement.volume = volume;
  }

  removeSong(index: number) {
    if (index == this.currentIndex) {
      this.selectNextSong();
    }

    this.playlist.splice(index, 1);

    if (this.playlist.length == 0){
      this.audioElement.pause();
    }
  }

  selectNextSong() {
    if(this.currentIndex < this.playlist.length - 1) {
      this.currentIndex++;
    }
    else {
      switch (this.playbackMode) {
        case RepeatPlayback.RepeatAll:
          this.currentIndex = 0;
          break;
        case RepeatPlayback.RepeateOne:
          break;
        case RepeatPlayback.NoRepeat:
          return;
      }
    }

    this.currentSong.set(this.playlist[this.currentIndex]);
  }

  handleError(event: Event) {
    console.log(event);
    this.removeSong(this.currentIndex);
    this.selectNextSong();
  }
}

enum RepeatPlayback {
  RepeatAll = 1,
  RepeateOne = 2,
  NoRepeat = 3
}
