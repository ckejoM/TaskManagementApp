import { Component } from '@angular/core';

@Component({
    standalone: true,
    selector: 'app-footer',
    template: `<div class="layout-footer">
        Task Management by
        <a href="https://github.com/ckejoM" target="_blank" rel="noopener noreferrer" class="text-primary font-bold hover:underline">Jovan Madzic</a>
    </div>`
})
export class AppFooter {}
