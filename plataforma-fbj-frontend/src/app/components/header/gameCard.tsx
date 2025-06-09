import Link from 'next/link'
import { RatingStars } from './RatingStars'

type Game = {
    id: number
    title: string
    genre: string
    releaseYear: number
    rating: number
    imageUrl: string
}

export const GameCard = ({ game }: { game: Game }) => {
    return (
        <div className="bg-gray-800 rounded-xl overflow-hidden shadow-lg hover:shadow-2xl transition-shadow">
            <div 
                className="h-48 bg-cover bg-center" 
                style={{ backgroundImage: `url(${game.imageUrl})` }}
            />
            <div className="p-4">
                <div className="flex justify-between items-start">
                    <div>
                        <h3 className="text-xl font-bold text-white">{game.title}</h3>
                        <p className="text-gray-400">{game.genre} • {game.releaseYear}</p>
                    </div>
                    <RatingStars rating={game.rating} />
                </div>
                
                <Link href={`/jogos/${game.id}`}>
                    <button className="mt-4 w-full bg-yellow-500 hover:bg-yellow-600 text-gray-900 font-bold py-2 px-4 rounded">
                        Ver Detalhes
                    </button>
                </Link>
            </div>
        </div>
    )
}