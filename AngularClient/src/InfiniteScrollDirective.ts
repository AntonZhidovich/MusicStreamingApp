import { Directive, ElementRef, EventEmitter, HostListener, Input, Output } from "@angular/core";
import { trace } from "console";

@Directive({
    selector: "[infinite-scroll]",
    standalone: true,
    host: {
        "(scroll)" : "onScroll($event.target);"
    }
})
export class InfiniteScrollDirective {
    @Input() edgeOffsetToLoad = 100; 
    @Output() loadElements = new EventEmitter();

    constructor(private elementRef: ElementRef) {}

    onScroll(target: HTMLElement) {
        let clientHeight = target.clientHeight; 
        let fullHeight = target.scrollHeight; 
        let scrollTop = target.scrollTop;
        let toBottom = fullHeight - scrollTop - clientHeight;

        if (toBottom < this.edgeOffsetToLoad) {
            this.loadElements.emit();
        }
        
    }

}