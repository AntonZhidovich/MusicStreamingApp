import { AddSongToReleaseModel } from "./AddSongToReleaseModel";

export class CreateReleaseModel {
    name = "";
    authorNames: string[] = [];
    songs: AddSongToReleaseModel[] = [];
}