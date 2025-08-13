import React from 'react';
import './TeamPage.css';

interface TeamMember {
  id: number;
  name: string;
  photo: string;
  linkedin: string;
  description: string;
}

const TeamPage: React.FC = () => {
  const teamMembers: TeamMember[] = [
    {
      id: 1,
      name: "Sarah Johnson",
      photo: "https://images.unsplash.com/photo-1580489944761-15a19d654956?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&h=400&q=80",
      linkedin: "linkedin.com/in/sarahjohnson",
      description: "CEO & Founder with 15+ years of experience in strategic leadership and business development."
    },
    {
      id: 2,
      name: "Michael Chen",
      photo: "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&h=400&q=80",
      linkedin: "linkedin.com/in/michaelchen",
      description: "CTO specializing in emerging technologies and digital transformation strategies."
    },
    {
      id: 3,
      name: "Emily Rodriguez",
      photo: "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&h=400&q=80",
      linkedin: "linkedin.com/in/emilyrodriguez",
      description: "Head of Design creating user-centered experiences that drive engagement and conversion."
    },
    {
      id: 4,
      name: "David Kim",
      photo: "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&h=400&q=80",
      linkedin: "linkedin.com/in/davidkim",
      description: "VP of Operations ensuring seamless delivery and exceptional customer satisfaction."
    }
  ];

  return (
    <div className="team-page">
      <h2 className="team-title">Our Team</h2>
      <div className="team-grid">
        {teamMembers.map((member) => (
          <div key={member.id} className="team-card">
            <div className="member-photo">
              <img src={member.photo} alt={member.name} />
            </div>
            <div className="member-info">
              <h3 className="member-name">{member.name}</h3>
              <a 
                href={`https://${member.linkedin}`} 
                target="_blank" 
                rel="noopener noreferrer"
                className="member-linkedin"
              >
                LinkedIn Profile
              </a>
              <p className="member-description">{member.description}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TeamPage;
