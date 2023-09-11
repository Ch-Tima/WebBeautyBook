import { Component, Input, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrls: ['./image-slider.component.css']
})
export class ImageSliderComponent implements OnInit, OnDestroy {
  
  @Input() public slides:Image[] = []; // An array of Image objects
  @Input() public time:number = 8000; // Time interval for slide transition

  currentIndex: number = 0; // Index of the currently displayed slide
  timeoutId?: number; // A timeout ID to control slide transitions

  ngOnInit(): void {
    this.resetTimer(); // Start the timer for slide transitions
  }

  ngOnDestroy() {
    window.clearTimeout(this.timeoutId);// Clear the timer when the component is destroyed
  }

  // Reset the timer for slide transitions
  resetTimer() {
    if (this.timeoutId) {
      window.clearTimeout(this.timeoutId);// Clear the existing timer
    }// Set a new timer to transition to the next slide after the specified time
    this.timeoutId = window.setTimeout(() => this.goToNext(), this.time);
  }

  // Navigate to the previous slide
  goToPrevious(): void {
    const isFirstSlide = this.currentIndex === 0;
    const newIndex = isFirstSlide ? this.slides.length - 1 : this.currentIndex - 1;
    this.resetTimer(); // Reset the timer for slide transitions
    this.currentIndex = newIndex; // Update the current slide index
  }

  // Navigate to the next slide
  goToNext(): void {
    const isLastSlide = this.currentIndex === this.slides.length - 1;
    const newIndex = isLastSlide ? 0 : this.currentIndex + 1;

    this.resetTimer();// Reset the timer for slide transitions
    this.currentIndex = newIndex;// Update the current slide index
  }

  // Navigate to a specific slide by index
  goToSlide(slideIndex: number): void {
    this.resetTimer(); // Reset the timer for slide transitions
    this.currentIndex = slideIndex; // Update the current slide index
  }

  // Get the URL of the currently displayed slide
  getCurrentSlideUrl() {
    if(this.slides.length == 0){
      console.warn("Slides is empty!")
      return;
    }
    // Return the URL of the current slide
    return `url('${this.slides[this.currentIndex].path}')`;
  }

}
// Define the Image class
class Image {
  public path:string|undefined = ''
}
