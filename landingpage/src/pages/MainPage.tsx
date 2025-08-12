import React from 'react';
import './MainPage.css';

const MainPage: React.FC = () => {
  return (
    <div className="main-page">
      <div className="hero-section">
        <div className="hero-content">
          <h1 className="hero-title">Welcome to Our Company</h1>
          <p className="hero-subtitle">Innovative solutions for tomorrow's challenges</p>
        </div>
        <div className="hero-image-container">
          <img 
            src="https://media.giphy.com/media/3o7abKhOpu0NwenH3O/giphy.gif" 
            alt="Animated company representation"
            className="hero-image"
          />
        </div>
      </div>
    </div>
  );
};

export default MainPage;
