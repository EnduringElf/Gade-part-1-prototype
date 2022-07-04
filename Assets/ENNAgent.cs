using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENNAgent : Agent
{
    public Map Map;

    public class Nueron 
    {
        public double Value;


    }

    public class NN
    {
        public List<double> InputLayer = new List<double>();
    }



    private void Start()
    {
        
       
    }

    private void GetInputNeurons(Map Map)
    {
        StaticMap staticMap = new StaticMap();
        staticMap.Verticel = Map.Verticle;
        staticMap.Horizontal = Map.Horizontal;
        staticMap.placements = new StaticPlacement[Map.Verticle, Map.Horizontal];

        for (int i = 0; i < Map.Verticle; i++)
        {
            for (int j = 0; j < Map.Horizontal; j++)
            {
                StaticPlacement placement = new StaticPlacement();
                placement.i = i;
                placement.j = j;

                if (Map.placements[i, j].GetFirstUnit() != null)
                {
                    Unit runit = Map.placements[i, j].GetFirstUnit();

                    StaticUnit unit = new StaticUnit();
                    unit.ATK = runit.ATK;
                    unit.DEF = runit.DEF;
                    unit.HP = runit.HP;
                    unit.homePlayer = runit.OwningAgent == this;

                    if (unit.homePlayer)
                        staticMap.homeUnit = unit;

                    if (!unit.homePlayer)
                        staticMap.enemyUnit = unit;

                    if (runit.HasItem())
                    {
                        unit.item = new StaticItem(runit.Item);
                    }

                    placement.unit1 = unit;
                }

                if (Map.placements[i, j].GetSecondUnit() != null)
                {
                    Unit runit = Map.placements[i, j].GetSecondUnit();

                    StaticUnit unit = new StaticUnit();
                    unit.ATK = runit.ATK;
                    unit.DEF = runit.DEF;
                    unit.HP = runit.HP;
                    unit.homePlayer = runit.OwningAgent == this;

                    if (unit.homePlayer)
                        staticMap.homeUnit = unit;

                    if (!unit.homePlayer)
                        staticMap.enemyUnit = unit;

                    if (runit.HasItem())
                    {
                        unit.item = new StaticItem(runit.Item);
                    }

                    placement.unit2 = unit;
                }

                if (Map.placements[i, j].item != null)
                    placement.item = new StaticItem(Map.placements[i, j].item);

                staticMap.placements[i, j] = placement;
            }
        }



    }

    public override void Action(Map Map)
    {
        GetInputNeurons(Map);
    }
}

internal class StaticMap
{
    public int Verticel;
    public int Horizontal;
    public StaticUnit homeUnit;
    public StaticUnit enemyUnit;
    public StaticPlacement[,] placements;

    public StaticMap Clone()
    {
        StaticMap map = new StaticMap();
        map.Verticel = Verticel;
        map.Horizontal = Horizontal;
        map.placements = new StaticPlacement[Verticel, Horizontal];

        for (int i = 0; i < map.Verticel; i++)
        {
            for (int j = 0; j < map.Horizontal; j++)
            {
                map.placements[i, j] = placements[i, j].Clone();

                if (placements[i, j].unit1 == homeUnit)
                {
                    map.homeUnit = map.placements[i, j].unit1;
                }
                if (placements[i, j].unit2 == homeUnit)
                {
                    map.homeUnit = map.placements[i, j].unit2;
                }

                if (placements[i, j].unit1 == enemyUnit)
                {
                    map.enemyUnit = map.placements[i, j].unit1;
                }
                if (placements[i, j].unit2 == enemyUnit)
                {
                    map.enemyUnit = map.placements[i, j].unit2;
                }
            }
        }

        return map;
    }

}

public class StaticPlacement
{

    public int i;
    public int j;
    public StaticUnit unit1;
    public StaticUnit unit2;
    public StaticItem item;

    public void PlaceUnit(StaticUnit unit)
    {
        if (unit1 == null)
            unit1 = unit;
        else if (unit2 == null)
            unit2 = unit;
    }

    public bool HasUnit(StaticUnit unit)
    {
        return unit1 == unit || unit2 == unit;
    }

    public StaticPlacement Clone()
    {
        StaticPlacement placement = new StaticPlacement();

        if (item != null)
            placement.item = item.Clone();

        if (unit1 != null)
            placement.unit1 = unit1.Clone();

        if (unit2 != null)
            placement.unit2 = unit2.Clone();

        placement.i = i;
        placement.j = j;

        return placement;
    }
}

public class StaticUnit
{
    public int HP;
    public int ATK;
    public int DEF;
    public bool homePlayer;
    public StaticItem item;

    public StaticUnit Clone()
    {
        StaticUnit unit = new StaticUnit();
        unit.HP = HP;
        unit.ATK = ATK;
        unit.DEF = DEF;
        unit.homePlayer = homePlayer;
        if (unit.item != null)
            unit.item = item.Clone();
        return unit;
    }
}

public class StaticItem
{
    public int ATK_Increase;
    public int DEF_Increase;
    public int HP_Restored;

    public StaticItem()
    {

    }

    public StaticItem(GameItem Item)
    {
        if (Item is Sword sword)
            ATK_Increase = sword.ATK_Increase;
        if (Item is Shield shield)
            DEF_Increase = shield.DEF_Increase;
        if (Item is HPPotion potion)
            HP_Restored = potion.HP_Restored;
    }

    public StaticItem Clone()
    {
        StaticItem item = new StaticItem();
        item.ATK_Increase = ATK_Increase;
        item.DEF_Increase = DEF_Increase;
        item.HP_Restored = HP_Restored;
        return item;
    }
}