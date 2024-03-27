export class Tokens{
    accessToken:string = "";
    refreshToken:string = "";

    constructor(access: string, refresh: string){
        this.accessToken = access;
        this.refreshToken = refresh;
    }
}