import React from 'react';
import './AboutPage.css';

const AboutPage: React.FC = () => {
  return (
    <div className="about-page">
      <div className="about-content">
        <div className="about-image-section">
          <img 
            src="https://media.giphy.com/media/3o7abKhOpu0NwenH3O/giphy.gif" 
            alt="About us representation"
            className="about-image"
          />
        </div>
        <div className="about-text-section">
          <h2 className="about-title">About Us</h2>
          <p className="about-description">
            We are a passionate team dedicated to delivering exceptional value to our clients. 
            Our mission is to innovate, inspire, and create solutions that make a difference.
          </p>
          <div className="contact-info">
            <h3>Get in Touch</h3>
            <div className="contact-item">
              <span className="contact-label">Phone:</span>
              <span className="contact-value">+1 (555) 123-4567</span>
            </div>
            <div className="contact-item">
              <span className="contact-label">Email:</span>
              <span className="contact-value">info@company.com</span>
            </div>
            <div className="contact-item">
              <span className="contact-label">Instagram:</span>
              <span className="contact-value">@companyname</span>
            </div>
            <div className="contact-item">
              <span className="contact-label">LinkedIn:</span>
              <span className="contact-value">linkedin.com/company/companyname</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AboutPage;
