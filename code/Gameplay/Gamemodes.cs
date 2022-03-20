﻿using System;
using Sandbox;

public partial class TDGame
{
	public enum GamemodeType
	{
		Unspecified,
		Cooperative,
		Competitive
	}

	[Net, Predicted]
	public GamemodeType GameType { get; private set; }

	[ServerCmd( "td_start_gametype" )]
	public static void HostSelectComp(GamemodeType gameType)
	{
		Event.Run( "td_evnt_play", gameType );
	}

	[Event( "td_evnt_play" )]
	public void PlayCoop(GamemodeType gameType)
	{
		if ( CurGameStatus != GameStatus.Idle )
			return;

		if(gameType == GamemodeType.Competitive)
		{
			if ( !CanPlayCompetitive() )
			{
				Log.Error( "Can't play competitive with just yourself" );
				return;
			}

			GameType = GamemodeType.Competitive;
			CurGameStatus = GameStatus.Starting;
			WaveTimer = 10.0f + Time.Now;
		} 
		else if (gameType == GamemodeType.Cooperative)
		{
			GameType = GamemodeType.Cooperative;
			CurGameStatus = GameStatus.Starting;
			WaveTimer = 10.0f + Time.Now;
		}
	}

	[AdminCmd( "Test" )]
	public static void DoStuff()
	{
		Log.Info( "WORKS" );
	}

	public bool CanPlayCompetitive()
	{
		if ( Client.All.Count < 2 )
			return false;

		return true;
	}

	public void SetUpTeams(TDPlayer player )
	{
		/*if ( player.CurrentBluePlayers().Count == 0 player.CurrentRedPlayers().Count == 0 )
		{
			if(Rand.Int(1, 2) == 1)
				player.JoinTeam( TDPlayer.Teams.Red );
			else
				player.JoinTeam( TDPlayer.Teams.Blue );
		}*/

		if ( player.CurrentBluePlayers().Count >= player.CurrentRedPlayers().Count )
			player.JoinTeam( TDPlayer.Teams.Red );
		else
			player.JoinTeam( TDPlayer.Teams.Blue );

	}

	public void AnnounceWinningTeam(string WinningTeam)
	{
		CurGameStatus = GameStatus.Post;

		if ( WinningTeam == "Red" )
			Log.Info( "Red team wins!" );
		else if ( WinningTeam == "Blue" )
			Log.Info( "Blue team wins!" );
	}
}
