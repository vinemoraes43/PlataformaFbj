import { Header } from '@/components/Header'
import Link from 'next/link'

export default function Home() {
  return (
    <>
      <Header />
      <main className="container mx-auto px-4 py-8">
        <div className="text-center py-12">
          <h1 className="text-4xl font-bold mb-6">Bem-vindo ao GameCritic!</h1>
          <p className="text-lg mb-8 max-w-2xl mx-auto">
            A plataforma onde jogadores compartilham suas opiniões sobre os melhores (e piores) jogos do mercado.
            Descubra novas experiências e ajude outros jogadores com suas avaliações.
          </p>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6 max-w-4xl mx-auto">
            <Card 
              title="Explorar Jogos" 
              description="Navegue por nossa biblioteca de jogos" 
              link="/jogos"
              icon="🎮"
            />
            <Card 
              title="Ver Avaliações" 
              description="Veja o que a comunidade está dizendo" 
              link="/avaliacoes"
              icon="⭐"
            />
            <Card 
              title="Ranking" 
              description="Descubra os jogos mais bem avaliados" 
              link="/ranking"
              icon="🏆"
            />
          </div>
        </div>
      </main>
    </>
  )
}

const Card = ({ title, description, link, icon }: { 
  title: string, 
  description: string, 
  link: string,
  icon: string
}) => (
  <Link href={link}>
    <div className="bg-white p-6 rounded-lg shadow-lg hover:shadow-xl transition-shadow cursor-pointer border border-gray-200">
      <div className="text-4xl mb-4">{icon}</div>
      <h2 className="text-xl font-semibold mb-3">{title}</h2>
      <p className="text-gray-600">{description}</p>
    </div>
  </Link>
)