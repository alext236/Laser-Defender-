using UnityEngine;
using System.Collections;
using System;

public class Status : MonoBehaviour, IEquatable<Status> {    

    public int statusID;

    public int StatusID {
        get { return statusID; }
        set { statusID = value; }
    }
    public int duration;

    public int Duration {
        get { return duration; }
        set { duration = value; }
    }

    public Status(int statusID, int duration) {
        this.statusID = statusID;
        this.duration = duration;
    }
    
    //adding a overloaded Equals method to compare with ID and duration (IEquatable interface)
    public override bool Equals(object o) {
        if (o == null) {
            return false;
        }

        Status stt = o as Status;
        if ((Status)stt == null) {
            return false;
        }

        return (statusID == stt.StatusID) && (duration == stt.Duration);
    }

    public bool Equals(Status stt) {
        if ((object)stt == null) {
            return false;
        }
        return (statusID == stt.StatusID) && (duration == stt.Duration);
    }

    public override int GetHashCode() {
        return duration * statusID + 10;
    }
}
