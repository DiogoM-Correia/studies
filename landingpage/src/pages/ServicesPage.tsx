import React from 'react';
import './ServicesPage.css';

interface Service {
  id: number;
  title: string;
  photo: string;
  description: string;
}

const ServicesPage: React.FC = () => {
  const services: Service[] = [
    {
      id: 1,
      title: "Web Development",
      photo: "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=400&h=300&fit=crop",
      description: "Custom web applications built with modern technologies, responsive design, and optimal performance."
    },
    {
      id: 2,
      title: "Mobile Apps",
      photo: "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=400&h=300&fit=crop",
      description: "Native and cross-platform mobile applications for iOS and Android with intuitive user experiences."
    },
    {
      id: 3,
      title: "Cloud Solutions",
      photo: "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=400&h=300&fit=crop",
      description: "Scalable cloud infrastructure, migration services, and DevOps automation for modern businesses."
    },
    {
      id: 4,
      title: "Digital Marketing",
      photo: "https://images.unsplash.com/photo-1557838923-2985c318be48?w=400&h=300&fit=crop",
      description: "Comprehensive digital marketing strategies including SEO, social media, and content marketing."
    }
  ];

  return (
    <div className="services-page">
      <h2 className="services-title">Our Services</h2>
      <div className="services-grid">
        {services.map((service) => (
          <div key={service.id} className="service-card">
            <div className="service-photo">
              <img src={service.photo} alt={service.title} />
            </div>
            <div className="service-content">
              <h3 className="service-title">{service.title}</h3>
              <p className="service-description">{service.description}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ServicesPage;
