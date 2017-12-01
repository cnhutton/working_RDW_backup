using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Room;

    public GameObject PurpleFeetPrefab;
    public GameObject ContinueButtonPrefab;
    public GameObject CompleteButtonPrefab;
    public GameObject LeftArrowPrefab;
    public GameObject RightArrowPrefab;
    public GameObject PaintingPrefab;
    public GameObject MotionSicknessPrefab;
    public GameObject LeftPathPrefab;
    public GameObject RightPathPrefab;
    public GameObject EndpointPrefab;
    public GameObject DiscernmentButtonsPrefab;

    public Transform NorthWallSpawn;
    public Transform EastWallSpawn;
    public Transform SouthWallSpawn;
    public Transform WestWallSpawn;
    public Transform PathSpawn;
    public Transform NorthEdgeSpawn;
    public Transform EastEdgeSpawn;
    public Transform SouthEdgeSpawn;
    public Transform WestEdgeSpawn;
    
    public void PurpleFeet(Vector3 spawnPoint)
    {
        Instantiate(PurpleFeetPrefab, spawnPoint, Quaternion.identity);
    }

    public void PurpleFeet(Edge edge) //
    {
        switch (edge)
        {
            case Edge.North:
                Instantiate(PurpleFeetPrefab, NorthEdgeSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Edge.East:
                Instantiate(PurpleFeetPrefab, EastEdgeSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            case Edge.South:
                Instantiate(PurpleFeetPrefab, SouthEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.West:
                Instantiate(PurpleFeetPrefab, WestEdgeSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void PurpleFeet(Edge edge, out GameObject feet) //
    {
        switch (edge)
        {
            case Edge.North:
                feet = Instantiate(PurpleFeetPrefab, NorthEdgeSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Edge.East:
                feet = Instantiate(PurpleFeetPrefab, EastEdgeSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            case Edge.South:
                feet = Instantiate(PurpleFeetPrefab, SouthEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.West:
                feet = Instantiate(PurpleFeetPrefab, WestEdgeSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void ProceedButton(Direction wall, out GameObject button)
    {
        switch (wall)
        {
            case Direction.North:
                button = (GameObject)Instantiate(CompleteButtonPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                button = (GameObject)Instantiate(CompleteButtonPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                button = (GameObject)Instantiate(CompleteButtonPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                button = (GameObject)Instantiate(CompleteButtonPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void ContinueButton(Direction wall, out GameObject button)
    {
        switch (wall)
        {
            case Direction.North:
                button = (GameObject)Instantiate(ContinueButtonPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                button = (GameObject)Instantiate(ContinueButtonPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                button = (GameObject)Instantiate(ContinueButtonPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                button = (GameObject)Instantiate(ContinueButtonPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void ContinueButton(Edge edge, out GameObject button)
    {
        switch (edge)
        {
            case Edge.North:
                button = (GameObject)Instantiate(ContinueButtonPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Edge.East:
                button = (GameObject)Instantiate(ContinueButtonPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Edge.South:
                button = (GameObject)Instantiate(ContinueButtonPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Edge.West:
                button = (GameObject)Instantiate(ContinueButtonPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void ArrowButton(bool spawnLeftArrow, out GameObject arrow)
    {
        arrow = Instantiate( LeftArrowPrefab, NorthWallSpawn.position, spawnLeftArrow ? Quaternion.Euler(0, 180, 0) : Quaternion.identity);
    }

    public void Painting(Direction wall, out GameObject painting)
    {
        switch (wall)
        {
            case Direction.North:
                painting = (GameObject)Instantiate(PaintingPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                painting = (GameObject)Instantiate(PaintingPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                painting = (GameObject)Instantiate(PaintingPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                painting = (GameObject)Instantiate(PaintingPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void MotionSicknessUI(out GameObject fms)
    {
       fms = (GameObject)Instantiate(MotionSicknessPrefab, NorthWallSpawn.position, Quaternion.identity);
    }

    public void MotionSicknessUI(Direction wall, out GameObject fms)
    {
        switch (wall)
        {
            case Direction.North:
                fms = (GameObject)Instantiate(MotionSicknessPrefab, NorthWallSpawn.position, Quaternion.identity);
                break;
            case Direction.East:
                fms = (GameObject)Instantiate(MotionSicknessPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            case Direction.South:
                fms = (GameObject)Instantiate(MotionSicknessPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Direction.West:
                fms = (GameObject)Instantiate(MotionSicknessPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void Path(bool spawnLeftTurn, Edge startEdge)
    {
        switch (startEdge)
        {
            case Edge.North:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.identity);
                break;
            case Edge.East:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            case Edge.South:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Edge.West:
                Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("startEdge", startEdge, null);
        }
    }

    public void Path(bool spawnLeftTurn, Edge startEdge, out GameObject path)
    {
        switch (startEdge)
        {
            case Edge.North:
                path = Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.identity);
                break;
            case Edge.East:
                path = Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            case Edge.South:
                path = Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Edge.West:
                path = Instantiate(spawnLeftTurn ? LeftPathPrefab : RightPathPrefab, PathSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("startEdge", startEdge, null);
        }
    }

    public void Endpoint(Edge edge)
    {
        switch (edge)
        {
            case Edge.North:
                Instantiate(EndpointPrefab, NorthEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.East:
                Instantiate(EndpointPrefab, EastEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.South:
                Instantiate(EndpointPrefab, SouthEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.West:
                Instantiate(EndpointPrefab, WestEdgeSpawn.position, Quaternion.identity);
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void Endpoint(Edge edge, out GameObject endpoint)
    {
        switch (edge)
        {
            case Edge.North:
                endpoint = Instantiate(EndpointPrefab, NorthEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.East:
                endpoint = Instantiate(EndpointPrefab, EastEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.South:
                endpoint = Instantiate(EndpointPrefab, SouthEdgeSpawn.position, Quaternion.identity);
                break;
            case Edge.West:
                endpoint = Instantiate(EndpointPrefab, WestEdgeSpawn.position, Quaternion.identity);
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void MoveEndpoint(Edge edge, GameObject g)
    {
        switch (edge)
        {
            case Edge.North:
                g.transform.position = NorthEdgeSpawn.position;
                break;
            case Edge.East:
                g.transform.position = EastEdgeSpawn.position;
                break;
            case Edge.South:
                g.transform.position = SouthEdgeSpawn.position;
                break;
            case Edge.West:
                g.transform.position = WestEdgeSpawn.position;
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void DiscernmentButtons(Direction wall, out GameObject buttons)
    {
        switch (wall)
        {
            case Direction.North:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, NorthWallSpawn.position, Quaternion.identity) as GameObject;
                break;
            case Direction.East:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                break;
            case Direction.South:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                break;
            case Direction.West:
                buttons = (GameObject)Instantiate(DiscernmentButtonsPrefab, WestWallSpawn.position, Quaternion.Euler(0, -90, 0)) as GameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }

    public void DiscernmentButtons(Edge edge, out GameObject buttons)
    {
        switch (edge)
        {
            case Edge.North:
                buttons = Instantiate(DiscernmentButtonsPrefab, NorthWallSpawn.position, Quaternion.identity);
                break;
            case Edge.East:
                buttons = Instantiate(DiscernmentButtonsPrefab, EastWallSpawn.position, Quaternion.Euler(0, 90, 0));
                break;
            case Edge.South:
                buttons = Instantiate(DiscernmentButtonsPrefab, SouthWallSpawn.position, Quaternion.Euler(0, 180, 0));
                break;
            case Edge.West:
                buttons = Instantiate(DiscernmentButtonsPrefab, WestEdgeSpawn.position, Quaternion.Euler(0, -90, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void MoveDiscernmentButtons(GameObject g, Edge edge)
    {
        switch (edge)
        {
            case Edge.North:
                g.transform.position = NorthWallSpawn.position;
                g.transform.rotation = Quaternion.identity;
                break;
            case Edge.East:
                g.transform.position = EastWallSpawn.position;
                g.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Edge.South:
                g.transform.position = SouthWallSpawn.position;
                g.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Edge.West:
                g.transform.position = WestWallSpawn.position;
                g.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException("edge", edge, null);
        }
    }

    public void RotatePainting(GameObject g, Direction wall)
    {
        switch (wall)
        {
            case Direction.North:
                g.transform.position = NorthWallSpawn.position;
                break;
            case Direction.East:
                g.transform.position = EastWallSpawn.position;
                break;
            case Direction.South:
                g.transform.position = SouthWallSpawn.position;
                break;
            case Direction.West:
                g.transform.position = WestWallSpawn.position;
                break;
            default:
                throw new ArgumentOutOfRangeException("wall", wall, null);
        }
    }
    
}
