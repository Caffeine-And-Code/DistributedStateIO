import type { Match } from "./Match";

export interface LeaderboardEntry {
  position: number;         
  userId: number;
  username: string;
  score: number;            
  recentMatches: Match[];   
}
